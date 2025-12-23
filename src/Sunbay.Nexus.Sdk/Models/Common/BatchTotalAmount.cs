using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Batch total amount information
    /// </summary>
    public class BatchTotalAmount
    {
        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }
        
        /// <summary>
        /// Total amount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }
    }
}

