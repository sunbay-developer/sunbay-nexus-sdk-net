using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
        
        private const string HeaderAuthorization = "Authorization";
        private const string HeaderContentType = "Content-Type";
        private const string HeaderRequestId = "X-Client-Request-Id";
        private const string HeaderTimestamp = "X-Timestamp";
        private const string ContentTypeJson = "application/json";
        private const string UserAgentPrefix = "Sunbay-Nexus-SDK-DotNet/";
        
        public HttpClientWrapper(NexusClientOptions options, ILogger? logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
            
            // Create HttpClient with custom handler
            var handler = new HttpClientHandler
            {
                MaxConnectionsPerServer = options.MaxConnectionsPerEndpoint
            };
            
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.BaseUrl),
                Timeout = options.Timeout
            };
            
            // Set default headers
            _httpClient.DefaultRequestHeaders.Add(HeaderAuthorization, $"{ApiConstants.AUTHORIZATION_BEARER_PREFIX}{options.ApiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"{UserAgentPrefix}1.0.0");
            
            // JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
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
            var content = new StringContent(json, Encoding.UTF8, ContentTypeJson);
            
            // Build full URL (baseUrl + path) like Java version
            var fullUrl = new Uri(_httpClient.BaseAddress!, path);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            
            AddCommonHeaders(httpRequest, ApiConstants.HTTP_METHOD_POST);
            
            // Explicitly set Content-Type header like Java version
            httpRequest.Content!.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentTypeJson);
            
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
            
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
        
        /// <summary>
        /// Add common headers to request
        /// </summary>
        private void AddCommonHeaders(HttpRequestMessage request, string method)
        {
            request.Headers.Add(HeaderRequestId, IdGenerator.GenerateRequestId());
#if NETSTANDARD2_0
            var timestamp = (long)(DateTimeOffset.UtcNow - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalMilliseconds;
            request.Headers.Add(HeaderTimestamp, timestamp.ToString());
#else
            request.Headers.Add(HeaderTimestamp, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
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
                if (!string.IsNullOrEmpty(requestBody))
                {
                    _logger.LogInformation("Request {Method} {Url} - Body: {Body}", requestMethod, requestUrl, requestBody);
                }
                else
                {
                    _logger.LogInformation("Request {Method} {Url}", requestMethod, requestUrl);
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
                    var result = ParseResponse<TResponse>(responseBody);
                    if (result == null)
                    {
                        throw new SunbayNetworkException(ApiConstants.MESSAGE_FAILED_PARSE_RESPONSE, false);
                    }
                    
                    if (!result.Success)
                    {
                        if (_logger?.IsEnabled(LogLevel.Error) == true)
                        {
                            _logger.LogError("API error {Method} {Url} - code: {Code}, msg: {Message}, traceId: {TraceId}",
                                requestMethod, requestUrl, result.Code, result.Message, result.TraceId);
                        }
                        throw new SunbayBusinessException(result.Code, result.Message, result.TraceId);
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
        /// </summary>
        private TResponse? ParseResponse<TResponse>(string responseBody)
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
                
                // Extract data field if exists
                TResponse? result;
                if (root.TryGetProperty(ApiConstants.JSON_FIELD_DATA, out var dataElement) && 
                    dataElement.ValueKind != JsonValueKind.Null)
                {
                    // Parse data field to response type
                    result = JsonSerializer.Deserialize<TResponse>(dataElement.GetRawText(), _jsonOptions);
                }
                else
                {
                    // No data field, parse entire response
                    result = JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
                }
                
                // Set base fields
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
            catch (Exception ex)
            {
                // Fallback to direct parsing
                if (_logger?.IsEnabled(LogLevel.Debug) == true)
                {
                    _logger.LogDebug(ex, "Failed to parse response with data field, fallback to direct parsing");
                }
                try
                {
                    return JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions);
                }
                catch (JsonException jsonEx)
                {
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
