using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Authorization amount information
    /// Supports: orderAmount, pricingCurrency only
    /// Used for: Auth, ForcedAuth, IncrementalAuth
    /// </summary>
    public class AuthAmount
    {
        /// <summary>
        /// Order amount (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public decimal OrderAmount { get; set; }
        
        /// <summary>
        /// Pricing currency (ISO 4217, required)
        /// </summary>
        [JsonPropertyName("pricingCurrency")]
        public string PricingCurrency { get; set; } = string.Empty;
    }
}

