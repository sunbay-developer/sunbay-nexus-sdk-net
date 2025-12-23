using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Refund response
    /// </summary>
    public class RefundResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this refund transaction, used for subsequent queries and notifications
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Reference order ID (same as original transaction for refund with reference, new refund reference order ID for refund without reference)
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string? ReferenceOrderId { get; set; }
        
        /// <summary>
        /// Transaction request ID for this refund, returned as-is from request
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
        
        /// <summary>
        /// Original transaction ID (only returned for refund with reference)
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction request ID (only returned for refund with reference)
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
    }
}

