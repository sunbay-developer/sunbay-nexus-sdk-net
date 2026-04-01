using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Amount breakdown for online checkout APIs (Hosted Payment Page and direct payment).
    /// Payable or charged total = orderAmount + taxAmount + surchargeAmount (when applicable).
    /// </summary>
    public class CheckoutAmount
    {
        /// <summary>
        /// Order amount in smallest currency unit (required).
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public long OrderAmount { get; set; }

        /// <summary>
        /// Tax amount in smallest currency unit (optional).
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public long? TaxAmount { get; set; }

        /// <summary>
        /// Surcharge amount in smallest currency unit (optional).
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public long? SurchargeAmount { get; set; }

        /// <summary>
        /// Price currency (ISO 4217, required).
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;
    }
}
