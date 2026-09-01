using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Merchant terminals query request. Uses token-based pagination (up to 100 terminals per page).
    /// </summary>
    public class MerchantTerminalsQueryRequest
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

        /// <summary>
        /// Pagination token returned by the previous response.
        /// Pass it back to fetch the next page. Omit on the first request.
        /// The token is an opaque string — do not parse or modify its contents.
        /// </summary>
        [JsonPropertyName("nextToken")]
        public string? NextToken { get; set; }
    }
}
