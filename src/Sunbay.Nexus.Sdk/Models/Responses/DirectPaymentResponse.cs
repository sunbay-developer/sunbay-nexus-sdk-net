using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Response data for online direct payment (<c>POST /v1/checkout/sale</c>).
    /// </summary>
    public class DirectPaymentResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY transaction ID (if returned)
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Reference order ID
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string? ReferenceOrderId { get; set; }

        /// <summary>
        /// Transaction request ID
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
    }
}
