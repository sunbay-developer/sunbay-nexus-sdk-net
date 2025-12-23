using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Post authorization response
    /// </summary>
    public class PostAuthResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this post authorization transaction, used for subsequent queries and notifications
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Transaction request ID for this post authorization, returned as-is from request
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
        
        /// <summary>
        /// Original authorization transaction ID, returned as-is from request (only returned when provided in request)
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original authorization transaction request ID, returned as-is from request (only returned when provided in request)
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
    }
}

