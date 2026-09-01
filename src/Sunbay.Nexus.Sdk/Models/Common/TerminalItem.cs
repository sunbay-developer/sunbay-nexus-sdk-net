using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Terminal information bound to a merchant
    /// </summary>
    public class TerminalItem
    {
        /// <summary>
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("sn")]
        public string? Sn { get; set; }

        /// <summary>
        /// Device vendor / manufacturer
        /// </summary>
        [JsonPropertyName("vendor")]
        public string? Vendor { get; set; }

        /// <summary>
        /// Device model
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// Time the terminal was bound to the merchant (ISO 8601)
        /// </summary>
        [JsonPropertyName("createTime")]
        public string? CreateTime { get; set; }

        /// <summary>
        /// TIDs assigned to this terminal by each payment channel (Processor).
        /// A terminal may be onboarded to multiple processors, and each processor assigns its own TID.
        /// Returns an empty array if no payment channel has been enabled for the terminal yet.
        /// </summary>
        [JsonPropertyName("tidList")]
        public List<TerminalTidItem>? TidList { get; set; }
    }
}
