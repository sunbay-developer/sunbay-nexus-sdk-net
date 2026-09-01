using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk.Constants;
using Sunbay.Nexus.Sdk.Exceptions;
using Sunbay.Nexus.Sdk.Models.Responses;
using Sunbay.Nexus.Sdk.Utilities;

namespace Sunbay.Nexus.Sdk.Http
{
    /// <summary>
    /// HTTP client wrapper for API communication
    /// </summary>
    internal class HttpClientWrapper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly NexusClientOptions _options;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger? _logger;
        private bool _disposed;
        
        public HttpClientWrapper(NexusClientOptions options, ILogger? logger = null)
        {
#if NETSTANDARD2_0
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options;
#else
            ArgumentNullException.ThrowIfNull(options);
            _options = options;
#endif
            _logger = logger;
            
            // Create HttpClient with custom handler
            // Note: For SDK libraries, managing HttpClient lifecycle directly is acceptable
            // as it provides full control over configuration and doesn't require DI container.
            // The HttpClient instance is properly disposed via IDisposable implementation.
#if NETSTANDARD2_0
            var handler = new HttpClientHandler
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerEndpoint
            };
#else
            // SocketsHttpHandler provides finer-grained pool control on .NET 6+:
            // - PooledConnectionLifetime: recycle to pick up DNS/LB changes
            // - PooledConnectionIdleTimeout: drop silently-dead idle connections
            // - ConnectTimeout: fail fast on unreachable hosts, independent of the overall request timeout
            var handler = new SocketsHttpHandler
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerEndpoint,
                PooledConnectionLifetime = options.PooledConnectionLifetime > TimeSpan.Zero
                    ? options.PooledConnectionLifetime
                    : Timeout.InfiniteTimeSpan,
                PooledConnectionIdleTimeout = options.PooledConnectionIdleTimeout > TimeSpan.Zero
                    ? options.PooledConnectionIdleTimeout
                    : Timeout.InfiniteTimeSpan,
                ConnectTimeout = options.ConnectTimeout > TimeSpan.Zero
                    ? options.ConnectTimeout
                    : Timeout.InfiniteTimeSpan,
                AutomaticDecompression = System.Net.DecompressionMethods.All
            };
#endif
            
            _httpClient = new HttpClient(handler, disposeHandler: true)
            {
                BaseAddress = new Uri(options.BaseUrl),
                Timeout = options.Timeout
            };
            
            // Set default headers
            _httpClient.DefaultRequestHeaders.Add(ApiConstants.HeaderAuthorization, $"{ApiConstants.AuthorizationBearerPrefix}{options.ApiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgentHelper.UserAgent);
            
            // JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                Converters = { new EnumMemberJsonConverterFactory() }
            };
        }

        /// <summary>
        /// Send POST request
        /// </summary>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest request,
            CancellationToken cancellationToken = default)
            where TResponse : BaseResponse
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, ApiConstants.ContentTypeJson);
            
            // Build full URL (baseUrl + path) like Java version
            var fullUrl = new Uri(_httpClient.BaseAddress!, path);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            
            AddCommonHeaders(httpRequest, ApiConstants.HttpMethodPost);
            
            // Explicitly set Content-Type header like Java version
            httpRequest.Content!.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ApiConstants.ContentTypeJson);
            
            return await ExecuteRequestAsync<TResponse>(httpRequest, json, cancellationToken)
                .ConfigureAwait(false);
        }
        
        /// <summary>
        /// Send GET request with query parameters from request object
        /// </summary>
        public async Task<TResponse> GetAsync<TRequest, TResponse>(
            string path,
            TRequest? request,
            CancellationToken cancellationToken = default)
            where TResponse : BaseResponse
        {
            var url = BuildUrlWithQuery(path, request);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            
            AddCommonHeaders(httpRequest, ApiConstants.HttpMethodGet);
            
            return await ExecuteWithRetryAsync(async () =>
            {
                return await ExecuteRequestAsync<TResponse>(httpRequest, null, cancellationToken)
                    .ConfigureAwait(false);
            }, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Build URL with query parameters from request object.
        /// Reads <see cref="JsonPropertyNameAttribute"/> to honor the same wire-format
        /// naming as JSON body serialization; falls back to camelCase if unset.
        /// </summary>
        private string BuildUrlWithQuery<T>(string path, T? request)
        {
            if (request == null)
            {
                return path;
            }

            var properties = TypeQueryCache.GetProperties(typeof(T));
            if (properties.Length == 0)
            {
                return path;
            }

            StringBuilder? sb = null;
            var first = !path.Contains('?');

            foreach (var (getter, name) in properties)
            {
                var value = getter(request!);
                if (value is null)
                {
                    continue;
                }

                sb ??= new StringBuilder(path.Length + 64).Append(path);
                sb.Append(first ? '?' : '&');
                first = false;
                sb.Append(name).Append('=').Append(Uri.EscapeDataString(value.ToString() ?? string.Empty));
            }

            return sb?.ToString() ?? path;
        }

        /// <summary>
        /// Add common headers to request
        /// </summary>
        private void AddCommonHeaders(HttpRequestMessage request, string method)
        {
            request.Headers.Add(ApiConstants.HeaderRequestId, Guid.NewGuid().ToString("N"));
#if NETSTANDARD2_0
            var timestamp = (long)(DateTimeOffset.UtcNow - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds;
            request.Headers.Add(ApiConstants.HeaderTimestamp, timestamp.ToString());
#else
            request.Headers.Add(ApiConstants.HeaderTimestamp, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
#endif
        }
        
        /// <summary>
        /// Execute HTTP request
        /// </summary>
        private async Task<TResponse> ExecuteRequestAsync<TResponse>(
            HttpRequestMessage request,
            string? requestBody,
            CancellationToken cancellationToken)
            where TResponse : BaseResponse
        {
            var requestUrl = request.RequestUri?.ToString() ?? string.Empty;
            var requestMethod = request.Method.Method;
            
            // Log request
            if (_logger?.IsEnabled(LogLevel.Information) == true)
            {
                var headers = FormatRequestHeaders(request);
                if (!string.IsNullOrEmpty(requestBody))
                {
                    _logger.LogInformation("Request {Method} {Url} - Headers: {Headers}, Body: {Body}", requestMethod, requestUrl, headers, requestBody);
                }
                else
                {
                    _logger.LogInformation("Request {Method} {Url} - Headers: {Headers}", requestMethod, requestUrl, headers);
                }
            }
            
            try
            {
                using var response = await _httpClient.SendAsync(request, cancellationToken)
                    .ConfigureAwait(false);
                
                var statusCode = (int)response.StatusCode;
#if NETSTANDARD2_0
                var responseBody = await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
#else
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);
#endif
                
                // Log response
                if (_logger?.IsEnabled(LogLevel.Information) == true)
                {
                    _logger.LogInformation("Response {Method} {Url} - Status: {StatusCode}, Body: {Body}", 
                        requestMethod, requestUrl, statusCode, responseBody);
                }
                
                if (statusCode >= ApiConstants.HttpStatusOkStart && statusCode < ApiConstants.HttpStatusOkEnd)
                {
                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new SunbayNetworkException(ApiConstants.MessageEmptyResponseBody, false);
                    }
                    
                    // Parse response with data field support
                    // If code != "0", ParseResponse will throw SunbayBusinessException
                    var result = ParseResponse<TResponse>(responseBody, requestMethod, requestUrl);
                    if (result == null)
                    {
                        throw new SunbayNetworkException(ApiConstants.MessageFailedParseResponse, false);
                    }
                    
                    return result;
                }
                else
                {
                    var errorMessage = BuildErrorMessage(statusCode, responseBody);
                    if (_logger?.IsEnabled(LogLevel.Error) == true)
                    {
                        _logger.LogError("HTTP error {Method} {Url} - Status: {StatusCode}, Message: {Message}",
                            requestMethod, requestUrl, statusCode, errorMessage);
                    }
                    throw new SunbayNetworkException(errorMessage, false);
                }
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                if (_logger?.IsEnabled(LogLevel.Warning) == true)
                {
                    _logger.LogWarning(ex, "Request timeout {Method} {Url}", requestMethod, requestUrl);
                }
                throw new SunbayNetworkException(ApiConstants.MessageRequestTimeout, ex, true);
            }
            catch (HttpRequestException ex)
            {
                if (_logger?.IsEnabled(LogLevel.Warning) == true)
                {
                    _logger.LogWarning(ex, "Network error {Method} {Url}", requestMethod, requestUrl);
                }
                throw new SunbayNetworkException($"Network error: {ex.Message}", ex, true);
            }
        }
        
        /// <summary>
        /// Parse response with data field support
        /// API returns: {"code":"0","msg":"Success","data":{...},"traceId":"..."}
        /// Need to extract data field and merge with base response
        /// If code != "0", throws SunbayBusinessException
        /// </summary>
        private TResponse? ParseResponse<TResponse>(string responseBody, string requestMethod, string requestUrl)
            where TResponse : BaseResponse
        {
            try
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;
                
                // Extract base fields (code, msg, traceId)
                var code = root.TryGetProperty(ApiConstants.JsonFieldCode, out var codeElement) 
                    ? codeElement.GetString() 
                    : null;
                var msg = root.TryGetProperty(ApiConstants.JsonFieldMsg, out var msgElement) 
                    ? msgElement.GetString() 
                    : null;
                var traceId = root.TryGetProperty(ApiConstants.JsonFieldTraceId, out var traceIdElement) 
                    ? traceIdElement.GetString() 
                    : null;
                
                // Check if code != "0", throw exception immediately
                if (code != ApiConstants.ResponseSuccessCode)
                {
                    if (_logger?.IsEnabled(LogLevel.Error) == true)
                    {
                        _logger.LogError("API error {Method} {Url} - code: {Code}, msg: {Message}, traceId: {TraceId}",
                            requestMethod, requestUrl, code ?? "null", msg ?? "null", traceId ?? "null");
                    }
                    throw new SunbayBusinessException(code ?? ApiConstants.ErrorCodeInvalidResponse, msg ?? "Unknown error", traceId);
                }
                
                // Extract data field if exists
                TResponse? result;
                if (root.TryGetProperty(ApiConstants.JsonFieldData, out var dataElement) && 
                    dataElement.ValueKind != JsonValueKind.Null)
                {
                    // Deserialize directly from the JsonElement to avoid a second JSON parse
                    // (previously: dataElement.GetRawText() -> allocate string -> re-parse)
                    result = dataElement.Deserialize<TResponse>(_jsonOptions);
                }
                else
                {
                    // No data field, deserialize from the root element (already parsed)
                    result = root.Deserialize<TResponse>(_jsonOptions);
                }
                
                // Set base fields (code, msg, traceId) from root level
                if (result != null)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        result.Code = code!;
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        result.Message = msg!;
                    }
                    if (!string.IsNullOrEmpty(traceId))
                    {
                        result.TraceId = traceId;
                    }
                }
                
                return result;
            }
            catch (SunbayBusinessException)
            {
                // Re-throw business exceptions
                throw;
            }
            catch (Exception ex)
            {
                // Fallback to direct parsing
                _logger?.LogWarning(ex, "Failed to parse response with data field extraction, fallback to direct parsing");
                try
                {
                    var result = JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
                    // Check code after fallback parsing
                    if (result != null && result.Code != ApiConstants.ResponseSuccessCode)
                    {
                        if (_logger?.IsEnabled(LogLevel.Error) == true)
                        {
                            _logger.LogError("API error {Method} {Url} - code: {Code}, msg: {Message}, traceId: {TraceId}",
                                requestMethod, requestUrl, result.Code, result.Message, result.TraceId ?? "null");
                        }
                        throw new SunbayBusinessException(result.Code, result.Message, result.TraceId);
                    }
                    return result;
                }
                catch (SunbayBusinessException)
                {
                    throw;
                }
                catch (JsonException jsonEx)
                {
                    _logger?.LogError(jsonEx, "Failed to parse response even with direct parsing");
                    throw new SunbayBusinessException(
                        ApiConstants.ErrorCodeInvalidResponse,
                        ApiConstants.MessageFailedParseResponse,
                        jsonEx);
                }
            }
        }
        
        /// <summary>
        /// Build error message from HTTP status code and response body
        /// </summary>
        private string BuildErrorMessage(int statusCode, string? responseBody)
        {
            var sb = new StringBuilder();
            sb.Append("HTTP ").Append(statusCode);
            
            if (statusCode >= ApiConstants.HttpStatusClientErrorStart && 
                statusCode < ApiConstants.HttpStatusClientErrorEnd)
            {
                sb.Append(" (Client Error)");
            }
            else if (statusCode >= ApiConstants.HttpStatusServerErrorStart)
            {
                sb.Append(" (Server Error)");
            }
            
            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                sb.Append(" - ").Append(responseBody);
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Execute with retry (exponential backoff)
        /// </summary>
        private async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> action,
            CancellationToken cancellationToken)
        {
            var retryCount = 0;
            var maxAttempts = _options.MaxRetries;
            
            while (true)
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (SunbayNetworkException ex) when (ex.IsRetryable && retryCount < maxAttempts)
                {
                    retryCount++;
                    if (_logger?.IsEnabled(LogLevel.Warning) == true)
                    {
                        _logger.LogWarning("Request failed after {Attempts} attempts: {Message}", retryCount, ex.Message);
                    }
                    
                    if (retryCount >= maxAttempts)
                    {
                        throw;
                    }
                    
                    if (_logger?.IsEnabled(LogLevel.Debug) == true)
                    {
                        _logger.LogDebug("Request failed, retrying ({Attempts}/{MaxAttempts}) after delay: {Message}",
                            retryCount, maxAttempts, ex.Message);
                    }
                    
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount)); // Exponential backoff
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
                catch (HttpRequestException ex) when (retryCount < maxAttempts)
                {
                    retryCount++;
                    if (_logger?.IsEnabled(LogLevel.Debug) == true)
                    {
                        _logger.LogDebug("Network error, retrying ({Attempts}/{MaxAttempts}): {Message}",
                            retryCount, maxAttempts, ex.Message);
                    }
                    
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
        }
        
        /// <summary>
        /// Format request headers for logging, with sensitive information masked
        /// </summary>
        private string FormatRequestHeaders(HttpRequestMessage request)
        {
            var headers = new List<string>();
            var processedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Add default headers from HttpClient (Authorization, User-Agent, etc.)
            foreach (var header in _httpClient.DefaultRequestHeaders)
            {
                var headerName = header.Key;
                var headerValues = string.Join(", ", header.Value);
                processedHeaders.Add(headerName);
                
                // Mask Authorization header
                if (string.Equals(headerName, ApiConstants.HeaderAuthorization, StringComparison.OrdinalIgnoreCase))
                {
                    // Format: "Authorization: Bearer ***" or "Authorization: ***"
                    if (headerValues.StartsWith(ApiConstants.AuthorizationBearerPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        headers.Add($"{headerName}: {ApiConstants.AuthorizationBearerPrefix}***");
                    }
                    else
                    {
                        headers.Add($"{headerName}: ***");
                    }
                }
                else
                {
                    headers.Add($"{headerName}: {headerValues}");
                }
            }
            
            // Add request-specific headers (skip if already in default headers)
            foreach (var header in request.Headers)
            {
                var headerName = header.Key;
                if (processedHeaders.Contains(headerName))
                {
                    continue; // Skip if already added from default headers
                }
                
                var headerValues = string.Join(", ", header.Value);
                headers.Add($"{headerName}: {headerValues}");
            }
            
            // Add content headers if exists
            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                {
                    var headerName = header.Key;
                    var headerValues = string.Join(", ", header.Value);
                    headers.Add($"{headerName}: {headerValues}");
                }
            }
            
            return string.Join("; ", headers);
        }
        
        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            
            _httpClient?.Dispose();
            _disposed = true;
        }
    }
}
