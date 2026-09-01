using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Tip configuration for on-screen tip collection
    /// </summary>
    public class TipConfig
    {
        /// <summary>
        /// Whether to use SUNBAY platform tip configuration. Default: false.
        /// When true, onScreenTip, tipMode, tipWithTax, and suggestions are ignored.
        /// </summary>
        [JsonPropertyName("useHostConfig")]
        public bool UseHostConfig { get; set; }

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
}
