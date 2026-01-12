using System;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Amount information
    /// All amounts are in the smallest currency unit (e.g., cents for USD, fen for CNY)
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }
        
        /// <summary>
        /// Transaction amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("transAmount")]
        public long? TransAmount { get; set; }
        
        /// <summary>
        /// Order amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public long? OrderAmount { get; set; }
        
        /// <summary>
        /// Tax amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public long? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public long? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Tip amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long? TipAmount { get; set; }
        
        /// <summary>
        /// Cashback amount in smallest currency unit
        /// </summary>
        [JsonPropertyName("cashbackAmount")]
        public long? CashbackAmount { get; set; }
        
    }
}
