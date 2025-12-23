using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Forced authorization response
    /// </summary>
    public class ForcedAuthResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this forced authorization transaction, used for subsequent queries and notifications
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

