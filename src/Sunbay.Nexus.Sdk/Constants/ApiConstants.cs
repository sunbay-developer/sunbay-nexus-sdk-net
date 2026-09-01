namespace Sunbay.Nexus.Sdk.Constants
{
    /// <summary>
    /// API constants for Sunbay Nexus SDK
    /// </summary>
    internal static class ApiConstants
    {
        /// <summary>Semi-integration API path prefix</summary>
        public const string SemiIntegrationPrefix = "/v1/semi-integration";

        /// <summary>Common API path prefix</summary>
        public const string CommonPrefix = "/v1";

        // API Paths
        public const string PathSale = SemiIntegrationPrefix + "/transaction/sale";
        public const string PathAuth = SemiIntegrationPrefix + "/transaction/auth";
        public const string PathForcedAuth = SemiIntegrationPrefix + "/transaction/forced-auth";
        public const string PathIncrementalAuth = SemiIntegrationPrefix + "/transaction/incremental-auth";
        public const string PathPostAuth = SemiIntegrationPrefix + "/transaction/post-auth";
        public const string PathRefund = SemiIntegrationPrefix + "/transaction/refund";
        public const string PathVoid = SemiIntegrationPrefix + "/transaction/void";
        public const string PathAbort = SemiIntegrationPrefix + "/transaction/abort";
        public const string PathTipAdjust = SemiIntegrationPrefix + "/transaction/tip-adjust";
        public const string PathQuery = CommonPrefix + "/transaction/query";
        public const string PathBatchClose = CommonPrefix + "/settlement/batch-close";
        public const string PathBatchQuery = CommonPrefix + "/settlement/batch-query";
        public const string PathBatchCloseList = CommonPrefix + "/settlement/batch-close-list";
        public const string PathMerchantQuery = CommonPrefix + "/merchant/query";
        public const string PathMerchantTerminalsQuery = CommonPrefix + "/merchant/terminals/query";

        /// <summary>Online Hosted Payment Page: create checkout session</summary>
        public const string PathCheckoutCreateSession = CommonPrefix + "/checkout/create-session";

        /// <summary>Online direct payment (e.g. wallet) without HPP session</summary>
        public const string PathCheckoutSale = CommonPrefix + "/checkout/sale";

        /// <summary>Online refund</summary>
        public const string PathCheckoutRefund = CommonPrefix + "/checkout/refund";

        /// <summary>Expire/close a checkout session</summary>
        public const string PathCheckoutExpireSession = CommonPrefix + "/checkout/expire-session";

        // Error Codes
        public const string ErrorCodeParameterError = "C17";
        public const string ErrorCodeNetworkError = "NETWORK_ERROR";
        public const string ErrorCodeTimeout = "TIMEOUT";
        public const string ErrorCodeServerError = "SERVER_ERROR";
        public const string ErrorCodeInvalidResponse = "INVALID_RESPONSE";

        // Default Values
        public const string DefaultBaseUrl = "https://open.sunbay.us";
        public const int DefaultTimeoutSeconds = 30;
        public const int DefaultConnectTimeoutSeconds = 10;
        public const int DefaultMaxRetries = 3;
        public const int DefaultMaxTotalConnections = 200;
        public const int DefaultMaxConnectionsPerEndpoint = 200;
        public const int DefaultPooledConnectionLifetimeSeconds = 300;
        public const int DefaultPooledConnectionIdleTimeoutSeconds = 120;

        // Error Messages
        public const string MessageApiKeyRequired = "API key cannot be null or empty";
        public const string MessageFailedParseResponse = "Failed to parse API response";
        public const string MessageResponseNull = "API response is null";
        public const string MessageServerError = "Server error";
        public const string MessageRequestFailed = "Request failed";
        public const string MessageRequestTimeout = "Request timeout";
        public const string MessageEmptyResponseBody = "Empty response body";
        public const string MessageInvalidUrl = "Invalid URL";

        // HTTP Methods
        public const string HttpMethodPost = "POST";
        public const string HttpMethodGet = "GET";

        // HTTP Status Codes
        public const int HttpStatusOkStart = 200;
        public const int HttpStatusOkEnd = 300;
        public const int HttpStatusClientErrorStart = 400;
        public const int HttpStatusClientErrorEnd = 500;
        public const int HttpStatusServerErrorStart = 500;

        // Response Success Code
        public const string ResponseSuccessCode = "0";

        // Authorization Header Prefix
        public const string AuthorizationBearerPrefix = "Bearer ";

        // JSON Field Names
        public const string JsonFieldCode = "code";
        public const string JsonFieldMsg = "msg";
        public const string JsonFieldData = "data";
        public const string JsonFieldTraceId = "traceId";

        // HTTP Header Names
        public const string HeaderAuthorization = "Authorization";
        public const string HeaderContentType = "Content-Type";
        public const string HeaderRequestId = "X-Client-Request-Id";
        public const string HeaderTimestamp = "X-Timestamp";

        // Content Types
        public const string ContentTypeJson = "application/json";
    }
}
