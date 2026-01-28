using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Enums;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Forced authorization request
    /// </summary>
    public class ForcedAuthRequest
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
        /// Reference order ID for the forced authorization transaction. Unique ID assigned by merchant system to identify this forced authorization transaction, 6-32 characters, can only contain numbers, uppercase/lowercase letters, _-\|*
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string ReferenceOrderId { get; set; } = string.Empty;
        
        /// <summary>
        /// Transaction request ID for this forced authorization transaction. Unique ID to identify this forced authorization transaction request, used as API idempotency control field
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;
        
        /// <summary>
        /// Amount information
        /// </summary>
        [JsonPropertyName("amount")]
        public AuthAmount Amount { get; set; } = new();
        
        /// <summary>
        /// Payment method information. Optional, recommended to omit for maximum flexibility
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        public PaymentMethodInfo? PaymentMethod { get; set; }
        
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
        /// Transaction expiration time, format: yyyy-MM-DDTHH:mm:ss+TIMEZONE (ISO 8601). Transaction will be closed if payment is not completed after this time. Minimum 3 minutes, maximum 1 day, default 1 day if not provided
        /// </summary>
        [JsonPropertyName("timeExpire")]
        public string? TimeExpire { get; set; }

        /// <summary>
        /// Receipt print option. NONE: do not print; MERCHANT: merchant copy only; CUSTOMER: customer copy only; BOTH: both copies. Default is NONE when not provided.
        /// </summary>
        [JsonPropertyName("printReceipt")]
        public PrintReceiptOption? PrintReceipt { get; set; }
    }
}

