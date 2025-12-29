using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Batch total amount information
    /// Amount is in the smallest currency unit (e.g., cents for USD, fen for CNY)
    /// </summary>
    public class BatchTotalAmount
    {
        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }
        
        /// <summary>
        /// Total amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }
    }
}

