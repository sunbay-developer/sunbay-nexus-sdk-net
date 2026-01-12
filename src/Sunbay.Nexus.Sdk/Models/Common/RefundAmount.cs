using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Refund amount information
    /// Supports: orderAmount, tipAmount, taxAmount, surchargeAmount, cashbackAmount
    /// All amounts are in the smallest currency unit (e.g., cents for USD, fen for CNY)
    /// </summary>
    public class RefundAmount
    {
        /// <summary>
        /// Order amount in smallest currency unit (required)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public long OrderAmount { get; set; }
        
        /// <summary>
        /// Tip amount in smallest currency unit (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long? TipAmount { get; set; }
        
        /// <summary>
        /// Tax amount in smallest currency unit (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public long? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount in smallest currency unit (optional, must be greater than or equal to 0).
        /// Note: Some processors may require surcharge to be refunded proportionally. Please contact technical support for detailed policies.
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public long? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Cashback amount in smallest currency unit (optional, must be greater than or equal to 0)
        /// </summary>
        [JsonPropertyName("cashbackAmount")]
        public long? CashbackAmount { get; set; }
        
        /// <summary>
        /// Price currency (ISO 4217, required)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;
    }
}

