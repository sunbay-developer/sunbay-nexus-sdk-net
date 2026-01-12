using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Batch query request
    /// </summary>
    public class BatchQueryRequest
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
        /// Terminal serial number. SUNBAY provided financial POS device serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
    }
}
