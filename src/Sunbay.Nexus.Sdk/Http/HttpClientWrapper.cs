using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
            var handler = new HttpClientHandler
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerEndpoint
            };
            
            _httpClient = new HttpClient(handler, disposeHandler: true)
            {
                BaseAddress = new Uri(options.BaseUrl),
                Timeout = options.Timeout
            };
            
            // Set default headers
            _httpClient.DefaultRequestHeaders.Add(ApiConstants.HEADER_AUTHORIZATION, $"{ApiConstants.AUTHORIZATION_BEARER_PREFIX}{options.ApiKey}");
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
            var content = new StringContent(json, Encoding.UTF8, ApiConstants.CONTENT_TYPE_JSON);
            
            // Build full URL (baseUrl + path) like Java version
            var fullUrl = new Uri(_httpClient.BaseAddress!, path);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            
            AddCommonHeaders(httpRequest, ApiConstants.HTTP_METHOD_POST);
            
            // Explicitly set Content-Type header like Java version
            httpRequest.Content!.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ApiConstants.CONTENT_TYPE_JSON);
            
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
            
            AddCommonHeaders(httpRequest, ApiConstants.HTTP_METHOD_GET);
            
            return await ExecuteWithRetryAsync(async () =>
            {
                return await ExecuteRequestAsync<TResponse>(httpRequest, null, cancellationToken)
                    .ConfigureAwait(false);
            }, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Build URL with query parameters from request object
        /// </summary>
        private string BuildUrlWithQuery<T>(string path, T? request)
        {
            if (request == null)
            {
                return path;
            }
            
            var queryParams = new List<string>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                var value = property.GetValue(request);
                if (value != null)
                {
                    var paramName = ConvertPropertyNameToParamName(property.Name);
                    var paramValue = Uri.EscapeDataString(value.ToString() ?? string.Empty);
                    queryParams.Add($"{paramName}={paramValue}");
                }
            }
            
            if (queryParams.Count == 0)
            {
                return path;
            }
            
            var separator = path.Contains('?') ? "&" : "?";
            return $"{path}{separator}{string.Join("&", queryParams)}";
        }
        
        /// <summary>
        /// Convert property name to query parameter name (camelCase)
        /// e.g., AppId -> appId, TransactionId -> transactionId
        /// </summary>
        private string ConvertPropertyNameToParamName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return propertyName;
            }
            
#if NETSTANDARD2_0
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
#else
            return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
#endif
        }
        
        /// <summary>
        /// Add common headers to request
        /// </summary>
        private void AddCommonHeaders(HttpRequestMessage request, string method)
        {
            request.Headers.Add(ApiConstants.HEADER_REQUEST_ID, IdGenerator.GenerateRequestId());
#if NETSTANDARD2_0
            var timestamp = (long)(DateTimeOffset.UtcNow - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds;
            request.Headers.Add(ApiConstants.HEADER_TIMESTAMP, timestamp.ToString());
#else
            request.Headers.Add(ApiConstants.HEADER_TIMESTAMP, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
#endif
            
            if (method == ApiConstants.HTTP_METHOD_POST)
            {
                // Content-Type is set on HttpContent, not header
            }
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
                
                if (statusCode >= ApiConstants.HTTP_STATUS_OK_START && statusCode < ApiConstants.HTTP_STATUS_OK_END)
                {
                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new SunbayNetworkException(ApiConstants.MESSAGE_EMPTY_RESPONSE_BODY, false);
                    }
                    
                    // Parse response with data field support
                    // If code != "0", ParseResponse will throw SunbayBusinessException
                    var result = ParseResponse<TResponse>(responseBody, requestMethod, requestUrl);
                    if (result == null)
                    {
                        throw new SunbayNetworkException(ApiConstants.MESSAGE_FAILED_PARSE_RESPONSE, false);
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
                throw new SunbayNetworkException(ApiConstants.MESSAGE_REQUEST_TIMEOUT, ex, true);
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
                var code = root.TryGetProperty(ApiConstants.JSON_FIELD_CODE, out var codeElement) 
                    ? codeElement.GetString() 
                    : null;
                var msg = root.TryGetProperty(ApiConstants.JSON_FIELD_MSG, out var msgElement) 
                    ? msgElement.GetString() 
                    : null;
                var traceId = root.TryGetProperty(ApiConstants.JSON_FIELD_TRACE_ID, out var traceIdElement) 
                    ? traceIdElement.GetString() 
                    : null;
                
                // Check if code != "0", throw exception immediately
                if (code != ApiConstants.RESPONSE_SUCCESS_CODE)
                {
                    if (_logger?.IsEnabled(LogLevel.Error) == true)
                    {
                        _logger.LogError("API error {Method} {Url} - code: {Code}, msg: {Message}, traceId: {TraceId}",
                            requestMethod, requestUrl, code ?? "null", msg ?? "null", traceId ?? "null");
                    }
                    throw new SunbayBusinessException(code ?? ApiConstants.ERROR_CODE_INVALID_RESPONSE, msg ?? "Unknown error", traceId);
                }
                
                // Extract data field if exists
                TResponse? result;
                if (root.TryGetProperty(ApiConstants.JSON_FIELD_DATA, out var dataElement) && 
                    dataElement.ValueKind != JsonValueKind.Null)
                {
                    // Parse data field to response type
                    var dataJson = dataElement.GetRawText();
                    result = JsonSerializer.Deserialize<TResponse>(dataJson, _jsonOptions);
                }
                else
                {
                    // No data field, parse entire response
                    result = JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
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
                    if (result != null && result.Code != ApiConstants.RESPONSE_SUCCESS_CODE)
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
                        ApiConstants.ERROR_CODE_INVALID_RESPONSE,
                        ApiConstants.MESSAGE_FAILED_PARSE_RESPONSE,
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
            
            if (statusCode >= ApiConstants.HTTP_STATUS_CLIENT_ERROR_START && 
                statusCode < ApiConstants.HTTP_STATUS_CLIENT_ERROR_END)
            {
                sb.Append(" (Client Error)");
            }
            else if (statusCode >= ApiConstants.HTTP_STATUS_SERVER_ERROR_START)
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
                if (string.Equals(headerName, ApiConstants.HEADER_AUTHORIZATION, StringComparison.OrdinalIgnoreCase))
                {
                    // Format: "Authorization: Bearer ***" or "Authorization: ***"
                    if (headerValues.StartsWith(ApiConstants.AUTHORIZATION_BEARER_PREFIX, StringComparison.OrdinalIgnoreCase))
                    {
                        headers.Add($"{headerName}: {ApiConstants.AUTHORIZATION_BEARER_PREFIX}***");
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
