using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Online refund amount information (smallest currency unit).
    /// </summary>
    public class OnlineRefundAmount
    {
        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;

        /// <summary>
        /// Total transaction amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public int? TotalAmount { get; set; }

        /// <summary>
        /// Order amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public int? OrderAmount { get; set; }

        /// <summary>
        /// Tax amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public int? TaxAmount { get; set; }

        /// <summary>
        /// Surcharge amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public int? SurchargeAmount { get; set; }

        /// <summary>
        /// Tip amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public int? TipAmount { get; set; }
    }
}
