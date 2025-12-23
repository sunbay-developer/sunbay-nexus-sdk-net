namespace Sunbay.Nexus.Sdk.Constants
{
    /// <summary>
    /// API constants for Sunbay Nexus SDK
    /// </summary>
    internal static class ApiConstants
    {
        /// <summary>
        /// Semi-integration API path prefix
        /// </summary>
        public const string SEMI_INTEGRATION_PREFIX = "/v1/semi-integration";

        /// <summary>
        /// Common API path prefix
        /// </summary>
        public const string COMMON_PREFIX = "/v1";

        // API Paths
        public const string PATH_SALE = SEMI_INTEGRATION_PREFIX + "/transaction/sale";
        public const string PATH_AUTH = SEMI_INTEGRATION_PREFIX + "/transaction/auth";
        public const string PATH_FORCED_AUTH = SEMI_INTEGRATION_PREFIX + "/transaction/forced-auth";
        public const string PATH_INCREMENTAL_AUTH = SEMI_INTEGRATION_PREFIX + "/transaction/incremental-auth";
        public const string PATH_POST_AUTH = SEMI_INTEGRATION_PREFIX + "/transaction/post-auth";
        public const string PATH_REFUND = SEMI_INTEGRATION_PREFIX + "/transaction/refund";
        public const string PATH_VOID = SEMI_INTEGRATION_PREFIX + "/transaction/void";
        public const string PATH_ABORT = SEMI_INTEGRATION_PREFIX + "/transaction/abort";
        public const string PATH_TIP_ADJUST = SEMI_INTEGRATION_PREFIX + "/transaction/tip-adjust";
        public const string PATH_QUERY = COMMON_PREFIX + "/transaction/query";
        public const string PATH_BATCH_CLOSE = SEMI_INTEGRATION_PREFIX + "/settlement/batch-close";
        
        // Error Codes
        public const string ERROR_CODE_PARAMETER_ERROR = "PARAMETER_ERROR";
        public const string ERROR_CODE_NETWORK_ERROR = "NETWORK_ERROR";
        public const string ERROR_CODE_TIMEOUT = "TIMEOUT";
        public const string ERROR_CODE_SERVER_ERROR = "SERVER_ERROR";
        public const string ERROR_CODE_INVALID_RESPONSE = "INVALID_RESPONSE";
        
        // Default Values
        public const string DEFAULT_BASE_URL = "https://open.sunbay.us";
        public const int DEFAULT_TIMEOUT_SECONDS = 30;
        public const int DEFAULT_MAX_RETRIES = 3;
        public const int DEFAULT_MAX_TOTAL_CONNECTIONS = 200;
        public const int DEFAULT_MAX_CONNECTIONS_PER_ENDPOINT = 20;

        // Error Messages
        public const string MESSAGE_API_KEY_REQUIRED = "API key cannot be null or empty";
        public const string MESSAGE_FAILED_PARSE_RESPONSE = "Failed to parse API response";
        public const string MESSAGE_RESPONSE_NULL = "API response is null";
        public const string MESSAGE_SERVER_ERROR = "Server error";
        public const string MESSAGE_REQUEST_FAILED = "Request failed";
        public const string MESSAGE_REQUEST_TIMEOUT = "Request timeout";
        public const string MESSAGE_EMPTY_RESPONSE_BODY = "Empty response body";
        public const string MESSAGE_INVALID_URL = "Invalid URL";
        
        // HTTP Methods
        public const string HTTP_METHOD_POST = "POST";
        public const string HTTP_METHOD_GET = "GET";
        
        // HTTP Status Codes
        public const int HTTP_STATUS_OK_START = 200;
        public const int HTTP_STATUS_OK_END = 300;
        public const int HTTP_STATUS_CLIENT_ERROR_START = 400;
        public const int HTTP_STATUS_CLIENT_ERROR_END = 500;
        public const int HTTP_STATUS_SERVER_ERROR_START = 500;
        
        // Response Success Code
        public const string RESPONSE_SUCCESS_CODE = "0";
        
        // Authorization Header Prefix
        public const string AUTHORIZATION_BEARER_PREFIX = "Bearer ";
        
        // JSON Field Names
        public const string JSON_FIELD_CODE = "code";
        public const string JSON_FIELD_MSG = "msg";
        public const string JSON_FIELD_DATA = "data";
        public const string JSON_FIELD_TRACE_ID = "traceId";
        
        // Getter Method Name Prefix Length
        public const int GETTER_METHOD_PREFIX_LENGTH = 3;
    }
}
