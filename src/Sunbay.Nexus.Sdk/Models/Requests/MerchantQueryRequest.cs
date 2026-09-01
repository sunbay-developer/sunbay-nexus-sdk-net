using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Merchant query request
    /// </summary>
    public class MerchantQueryRequest
    {
        /// <summary>
        /// Application ID
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// SUNBAY platform merchant unique identifier.
        /// Format: 11-character alphanumeric string starting with M.
        /// Note: This is not the MID assigned by a payment processor
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;
    }
}
