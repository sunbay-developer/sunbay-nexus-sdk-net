using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Enums;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Sale transaction request
    /// </summary>
    public class SaleRequest
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
        /// Reference order ID for the sale transaction
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string ReferenceOrderId { get; set; } = string.Empty;
        
        /// <summary>
        /// Unique request identifier for this sale transaction
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;
        
        /// <summary>
        /// Amount information
        /// </summary>
        [JsonPropertyName("amount")]
        public SaleAmount Amount { get; set; } = new();

        /// <summary>
        /// Payment method information. Optional.
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        public PaymentMethodInfo? PaymentMethod { get; set; }

        /// <summary>
        /// Card network type for card acceptance. Only effective when paymentMethod.category is CARD; when not specified, system auto-detects.
        /// </summary>
        [JsonPropertyName("cardNetworkType")]
        public CardNetworkType? CardNetworkType { get; set; }
        
        /// <summary>
        /// Product description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Additional data
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }
        
        /// <summary>
        /// Asynchronous notification URL
        /// </summary>
        [JsonPropertyName("notifyUrl")]
        public string? NotifyUrl { get; set; }
        
        /// <summary>
        /// Transaction expiration time (ISO 8601 format)
        /// </summary>
        [JsonPropertyName("timeExpire")]
        public string? TimeExpire { get; set; }

        /// <summary>
        /// Receipt print option. NONE: do not print; MERCHANT: merchant copy only; CUSTOMER: customer copy only; BOTH: both copies. Default is NONE when not provided.
        /// </summary>
        [JsonPropertyName("printReceipt")]
        public PrintReceiptOption? PrintReceipt { get; set; }

        /// <summary>
        /// Signature entry location. Optional values: ON_SCREEN (terminal screen signature), ON_RECEIPT (receipt signature).
        /// When omitted, the backend default configuration is used.
        /// </summary>
        [JsonPropertyName("signatureEntryLocation")]
        public string? SignatureEntryLocation { get; set; }
    }
}
