using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Tip configuration for on-screen tip collection
    /// </summary>
    public class TipConfig
    {
        private List<TipSuggestions>? _suggestions;

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
        public List<TipSuggestions>? Suggestions
        {
            get => _suggestions;
            set
            {
                if (value != null && value.Count > 3)
                {
                    throw new ArgumentException("Tip suggestions support up to 3 items.", nameof(Suggestions));
                }

                _suggestions = value;
            }
        }
    }
}
