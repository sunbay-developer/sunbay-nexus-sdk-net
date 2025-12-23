using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Void response
    /// </summary>
    public class VoidResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this void transaction, used for subsequent queries and notifications
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Transaction request ID for this void, returned as-is from request
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
        
        /// <summary>
        /// Original transaction ID
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction request ID
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
    }
}

