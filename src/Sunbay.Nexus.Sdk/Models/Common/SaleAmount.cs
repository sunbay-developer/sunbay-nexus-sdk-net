using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Sale transaction amount information
    /// </summary>
    public class SaleAmount
    {
        /// <summary>
        /// Order amount (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public decimal OrderAmount { get; set; }
        
        /// <summary>
        /// Tip amount (optional)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public decimal? TipAmount { get; set; }
        
        /// <summary>
        /// Tax amount (optional)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public decimal? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount (optional)
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public decimal? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Cashback amount (optional)
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
