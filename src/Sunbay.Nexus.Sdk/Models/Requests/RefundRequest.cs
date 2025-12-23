using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Refund request
    /// </summary>
    public class RefundRequest
    {
        /// <summary>
        /// Application ID
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;
        
        /// <summary>
        /// Merchant ID
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;
        
        /// <summary>
        /// Original transaction ID to refund. Either originalTransactionId or originalTransactionRequestId is required for refund with reference. Both must be empty for refund without reference. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction request ID to refund. Either originalTransactionId or originalTransactionRequestId is required for refund with reference. Both must be empty for refund without reference. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
        
        /// <summary>
        /// Reference order ID. Required for refund without reference, used to associate business records in merchant system. Not required for refund with reference, system will automatically associate with original transaction's reference order ID
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string? ReferenceOrderId { get; set; }
        
        /// <summary>
        /// Transaction request ID for this refund transaction. Unique ID to identify this refund request, used as API idempotency control field
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;
        
        /// <summary>
        /// Amount information
        /// </summary>
        [JsonPropertyName("amount")]
        public RefundAmount Amount { get; set; } = new();
        
        /// <summary>
        /// Payment method information. Only available for refund without reference. Optional, recommended to omit for maximum flexibility
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        public PaymentMethodInfo? PaymentMethod { get; set; }
        
        /// <summary>
        /// Refund reason description. Should be a real description representing the refund reason
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Terminal serial number. SUNBAY provided financial POS device serial number for reading bank cards and processing PIN security operations
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Additional data, returned as-is, recommended to use JSON format
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }
        
        /// <summary>
        /// Asynchronous notification URL
        /// </summary>
        [JsonPropertyName("notifyUrl")]
        public string? NotifyUrl { get; set; }
        
        /// <summary>
        /// Transaction expiration time, format: yyyy-MM-DDTHH:mm:ss+TIMEZONE (ISO 8601). Transaction will be closed if payment is not completed after this time. Minimum 3 minutes, maximum 1 day, default 1 day if not provided. Only used for refund without reference (requires customer operation on terminal), not needed for refund with reference
        /// </summary>
        [JsonPropertyName("timeExpire")]
        public string? TimeExpire { get; set; }
    }
}

