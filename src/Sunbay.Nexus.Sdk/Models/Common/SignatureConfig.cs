using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Signature configuration
    /// </summary>
    public class SignatureConfig
    {
        /// <summary>
        /// Whether to use SUNBAY platform signature configuration. Default: true.
        /// When true, entryLocation and threshold are ignored.
        /// </summary>
        [JsonPropertyName("useHostConfig")]
        public bool UseHostConfig { get; set; } = true;

        /// <summary>
        /// Signature entry location when useHostConfig is false.
        /// Optional values: ON_SCREEN, ON_RECEIPT, NONE.
        /// </summary>
        [JsonPropertyName("entryLocation")]
        public string? EntryLocation { get; set; }

        /// <summary>
        /// Signature threshold in smallest currency unit.
        /// Effective only when useHostConfig is false and entryLocation is not NONE.
        /// If omitted, all amounts require signature.
        /// </summary>
        [JsonPropertyName("threshold")]
        public long? Threshold { get; set; }
    }
}
