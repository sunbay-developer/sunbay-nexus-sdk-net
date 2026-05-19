using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Tip configuration for on-screen tip collection
    /// </summary>
    public class TipConfig
    {
        /// <summary>
        /// Whether to enable on-screen tip
        /// </summary>
        [JsonPropertyName("onScreenTip")]
        public bool OnScreenTip { get; set; }
        
        /// <summary>
        /// Tip mode: ON_SALE or AFTER_SALE
        /// </summary>
        [JsonPropertyName("tipMode")]
        public string? TipMode { get; set; }
        
        /// <summary>
        /// Whether tip includes tax
        /// </summary>
        [JsonPropertyName("tipWithTax")]
        public bool TipWithTax { get; set; }
        
        /// <summary>
        /// Tip suggestions configuration
        /// </summary>
        [JsonPropertyName("suggestions")]
        public TipSuggestions? Suggestions { get; set; }
    }
    
    /// <summary>
    /// Tip suggestions configuration
    /// </summary>
    public class TipSuggestions
    {
        /// <summary>
        /// Fee mode: RATE or AMOUNT
        /// </summary>
        [JsonPropertyName("feeMode")]
        public string? FeeMode { get; set; }
        
        /// <summary>
        /// Suggested tip values (percentages when feeMode is RATE, amounts when feeMode is AMOUNT)
        /// </summary>
        [JsonPropertyName("values")]
        public int[]? Values { get; set; }
    }
}
