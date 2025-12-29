using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Tip adjust request
    /// </summary>
    public class TipAdjustRequest
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
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Original transaction ID to adjust tip. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction request ID to adjust tip. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
        
        /// <summary>
        /// New tip amount after adjustment, in smallest currency unit (e.g., cents for USD, fen for CNY)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long TipAmount { get; set; }
        
        /// <summary>
        /// Additional data, returned as-is, recommended to use JSON format
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }
    }
}

