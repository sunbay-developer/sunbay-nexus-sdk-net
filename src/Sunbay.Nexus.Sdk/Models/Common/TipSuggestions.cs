using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Tip suggestion option.
    /// </summary>
    public class TipSuggestions
    {
        private List<string>? _names;
        private List<double>? _values;

        /// <summary>
        /// Optional display names for the tip option.
        /// </summary>
        [JsonPropertyName("names")]
        public List<string>? Names
        {
            get => _names;
            set
            {
                ValidateCount(value, nameof(Names));
                ValidateMatching(value, _values, nameof(Names));
                _names = value;
            }
        }

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
                ValidateCount(value, nameof(Values));
                ValidateMatching(_names, value, nameof(Values));
                _values = value;
            }
        }

        private static void ValidateCount<T>(ICollection<T>? value, string parameterName)
        {
            if (value != null && value.Count > 3)
            {
                throw new ArgumentException("Tip suggestions support up to 3 items.", parameterName);
            }
        }

        private static void ValidateMatching(List<string>? names, List<double>? values, string parameterName)
        {
            if (names != null && names.Count > 0 && values != null && names.Count != values.Count)
            {
                throw new ArgumentException("Tip suggestion names must match values in length.", parameterName);
            }
        }
    }
}
