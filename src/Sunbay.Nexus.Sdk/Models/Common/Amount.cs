using System;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Amount information
    /// Note: API returns amounts as strings, so we use string properties with helper methods to parse
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }
        
        /// <summary>
        /// Transaction amount (as string from API)
        /// </summary>
        [JsonPropertyName("transAmount")]
        public string? TransAmount { get; set; }
        
        /// <summary>
        /// Order amount (as string from API)
        /// </summary>
        [JsonPropertyName("orderAmount")]
        public string? OrderAmount { get; set; }
        
        /// <summary>
        /// Tax amount (as string from API)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public string? TaxAmount { get; set; }
        
        /// <summary>
        /// Surcharge amount (as string from API)
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public string? SurchargeAmount { get; set; }
        
        /// <summary>
        /// Tip amount (as string from API)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public string? TipAmount { get; set; }
        
        /// <summary>
        /// Cashback amount (as string from API)
        /// </summary>
        [JsonPropertyName("cashbackAmount")]
        public string? CashbackAmount { get; set; }
        
        /// <summary>
        /// Pricing currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("pricingCurrency")]
        public string? PricingCurrency { get; set; }
        
        /// <summary>
        /// Get transaction amount as decimal
        /// </summary>
        public decimal? GetTransAmountAsDecimal()
        {
            return ParseDecimal(TransAmount);
        }
        
        /// <summary>
        /// Get order amount as decimal
        /// </summary>
        public decimal? GetOrderAmountAsDecimal()
        {
            return ParseDecimal(OrderAmount);
        }
        
        /// <summary>
        /// Get tax amount as decimal
        /// </summary>
        public decimal? GetTaxAmountAsDecimal()
        {
            return ParseDecimal(TaxAmount);
        }
        
        /// <summary>
        /// Get surcharge amount as decimal
        /// </summary>
        public decimal? GetSurchargeAmountAsDecimal()
        {
            return ParseDecimal(SurchargeAmount);
        }
        
        /// <summary>
        /// Get tip amount as decimal
        /// </summary>
        public decimal? GetTipAmountAsDecimal()
        {
            return ParseDecimal(TipAmount);
        }
        
        /// <summary>
        /// Get cashback amount as decimal
        /// </summary>
        public decimal? GetCashbackAmountAsDecimal()
        {
            return ParseDecimal(CashbackAmount);
        }
        
        private static decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            
            if (decimal.TryParse(value, out var result))
            {
                return result;
            }
            
            return null;
        }
    }
}
