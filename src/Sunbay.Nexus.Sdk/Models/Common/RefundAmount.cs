using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Refund amount information
    /// Supports: orderAmount, tipAmount, taxAmount, surchargeAmount, cashbackAmount
    /// </summary>
    public class RefundAmount
    {
        /// <summary>
        /// Order amount (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public decimal OrderAmount { get; set; }
        
        /// <summary>
        /// Tip amount (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public decimal? TipAmount { get; set; }
        
        /// <summary>
        /// Tax amount (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public decimal? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount (optional, must be greater than or equal to 0).
        /// Note: Some processors may require surcharge to be refunded proportionally. Please contact technical support for detailed policies.
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public decimal? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Cashback amount (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("cashbackAmount")]
        public decimal? CashbackAmount { get; set; }
        
        /// <summary>
        /// Pricing currency (ISO 4217, required)
        /// </summary>
        [JsonPropertyName("pricingCurrency")]
        public string PricingCurrency { get; set; } = string.Empty;
    }
}

