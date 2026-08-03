using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Request to expire/close a checkout session (<c>POST /v1/checkout/expire-session</c>).
    /// </summary>
    public class ExpireCheckoutSessionRequest
    {
        /// <summary>
        /// Application ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// Merchant ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// The session ID to expire, as returned in the create-session response
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// Optional reason for closing the session
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
