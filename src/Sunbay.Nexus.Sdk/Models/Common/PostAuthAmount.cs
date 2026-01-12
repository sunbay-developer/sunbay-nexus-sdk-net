using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Post authorization amount information
    /// Supports: orderAmount, tipAmount, taxAmount, surchargeAmount
    /// Does NOT support: cashbackAmount
    /// All amounts are in the smallest currency unit (e.g., cents for USD, fen for CNY)
    /// </summary>
    public class PostAuthAmount
    {
        /// <summary>
        /// Order amount in smallest currency unit (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public long OrderAmount { get; set; }
        
        /// <summary>
        /// Tip amount in smallest currency unit (optional)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long? TipAmount { get; set; }
        
        /// <summary>
        /// Tax amount in smallest currency unit (optional)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public long? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount in smallest currency unit (optional)
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public long? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Price currency (ISO 4217, required)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;
    }
}

