using System.Text.Json.Serialization;
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
    }
}
