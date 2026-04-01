using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Response data for create checkout session (Hosted Payment Page).
    /// </summary>
    public class CreateCheckoutSessionResponse : BaseResponse
    {
        /// <summary>
        /// URL to redirect the customer to the Hosted Payment Page
        /// </summary>
        [JsonPropertyName("checkoutUrl")]
        public string? CheckoutUrl { get; set; }

        /// <summary>
        /// Session expiry time (e.g. ISO 8601). Session lifetime is typically 30 minutes from a successful response.
        /// </summary>
        [JsonPropertyName("expiresAt")]
        public string? ExpiresAt { get; set; }

        /// <summary>
        /// Transaction request ID echoed from the request (if returned by API)
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }

        /// <summary>
        /// Checkout or session identifier (if returned by API)
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }
    }
}
