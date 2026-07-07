using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Tip suggestion option.
    /// </summary>
    public class TipSuggestions
    {
        private List<double>? _values;

        /// <summary>
        /// Optional display name for the tip option.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Fee mode: RATE or AMOUNT.
        /// </summary>
        [JsonPropertyName("feeMode")]
        public string? FeeMode { get; set; }

        /// <summary>
        /// Suggested tip values (percentages when feeMode is RATE, amounts when feeMode is AMOUNT).
        /// Up to 3 values are supported.
        /// </summary>
        [JsonPropertyName("values")]
        public List<double>? Values
        {
            get => _values;
            set
            {
                if (value != null && value.Count > 3)
                {
                    throw new System.ArgumentException("Tip suggestions support up to 3 values.", nameof(value));
                }

                _values = value;
            }
        }
    }
}
