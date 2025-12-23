using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Abort request
    /// </summary>
    public class AbortRequest
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
        /// Original transaction ID to abort. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction request ID to abort. Either originalTransactionId or originalTransactionRequestId is required. If both are provided, originalTransactionId takes priority
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
        
        /// <summary>
        /// Terminal serial number. SUNBAY provided financial POS device serial number for reading bank cards and processing PIN security operations
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Abort reason description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Additional data, returned as-is, recommended to use JSON format
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }
    }
}

