using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Authorization amount information
    /// Supports: orderAmount, priceCurrency only
    /// Used for: Auth, ForcedAuth, IncrementalAuth
    /// All amounts are in the smallest currency unit (e.g., cents for USD, fen for CNY)
    /// </summary>
    public class AuthAmount
    {
        /// <summary>
        /// Order amount in smallest currency unit (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public long OrderAmount { get; set; }
        
        /// <summary>
        /// Price currency (ISO 4217, required)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;
    }
}

