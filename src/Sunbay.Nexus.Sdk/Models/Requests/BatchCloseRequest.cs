using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Batch close request
    /// </summary>
    public class BatchCloseRequest
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
        /// Batch close request unique identifier. Unique ID to identify this batch close request, used as API idempotency control field, can be used later to query batch close results
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;
        
        /// <summary>
        /// Terminal serial number. SUNBAY provided financial POS device serial number for reading bank cards and processing PIN security operations
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Batch close description
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

