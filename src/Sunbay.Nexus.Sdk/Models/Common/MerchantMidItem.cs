using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// MID information assigned by a payment processor
    /// </summary>
    public class MerchantMidItem
    {
        /// <summary>
        /// Payment channel code identifying the processor this MID belongs to
        /// </summary>
        [JsonPropertyName("channelCode")]
        public string? ChannelCode { get; set; }

        /// <summary>
        /// Payment channel display name (for presentation only)
        /// </summary>
        [JsonPropertyName("channelName")]
        public string? ChannelName { get; set; }

        /// <summary>
        /// Merchant Identification Number (MID) assigned by the payment processor.
        /// Note: this differs from the SUNBAY platform merchantId
        /// </summary>
        [JsonPropertyName("mid")]
        public string? Mid { get; set; }
    }
}
