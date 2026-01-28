using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Enums;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Incremental authorization request
    /// </summary>
    public class IncrementalAuthRequest
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
        /// Original authorization transaction ID to increase authorization amount. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original authorization transaction request ID to increase authorization amount. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
        
        /// <summary>
        /// Transaction request ID for this incremental authorization transaction. Unique ID to identify this incremental authorization request, used as API idempotency control field
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;
        
        /// <summary>
        /// Amount information
        /// </summary>
        [JsonPropertyName("amount")]
        public AuthAmount Amount { get; set; } = new();
        
        /// <summary>
        /// Product description. Should be a real description representing the product information, may be displayed on some payment App billing pages
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
        /// Receipt print option. NONE: do not print; MERCHANT: merchant copy only; CUSTOMER: customer copy only; BOTH: both copies. Default is NONE when not provided.
        /// </summary>
        [JsonPropertyName("printReceipt")]
        public PrintReceiptOption? PrintReceipt { get; set; }
    }
}

