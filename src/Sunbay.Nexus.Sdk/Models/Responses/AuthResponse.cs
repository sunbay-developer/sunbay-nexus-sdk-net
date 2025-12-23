using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Authorization response
    /// </summary>
    public class AuthResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this authorization transaction, used for subsequent queries, notifications, and post authorization/incremental authorization operations
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Reference order ID, returned as-is from request
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string? ReferenceOrderId { get; set; }
        
        /// <summary>
        /// Transaction request ID, returned as-is from request
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
    }
}

