using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Response for expire checkout session.
    /// </summary>
    public class ExpireCheckoutSessionResponse : BaseResponse
    {
        /// <summary>
        /// Echo of the session ID from the request
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }

        /// <summary>
        /// Session status after expiration, always "EXPIRED"
        /// </summary>
        [JsonPropertyName("sessionStatus")]
        public string? SessionStatus { get; set; }

        /// <summary>
        /// SUNBAY transaction ID associated with the session (if generated during session creation)
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Transaction request ID used when creating the session
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }

        /// <summary>
        /// Session expiration time in ISO 8601 format (e.g. "2023-11-19T10:30:00+08:00")
        /// </summary>
        [JsonPropertyName("expiredAt")]
        public string? ExpiredAt { get; set; }
    }
}
