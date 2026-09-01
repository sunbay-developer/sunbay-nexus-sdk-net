using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// TID information assigned by a payment processor to a terminal
    /// </summary>
    public class TerminalTidItem
    {
        /// <summary>
        /// Payment channel code identifying the processor this TID belongs to
        /// </summary>
        [JsonPropertyName("channelCode")]
        public string? ChannelCode { get; set; }

        /// <summary>
        /// Payment channel display name (for presentation only)
        /// </summary>
        [JsonPropertyName("channelName")]
        public string? ChannelName { get; set; }

        /// <summary>
        /// Terminal Identification Number (TID) assigned by the payment processor.
        /// Note: this differs from the terminal serial number (sn)
        /// </summary>
        [JsonPropertyName("tid")]
        public string? Tid { get; set; }
    }
}
