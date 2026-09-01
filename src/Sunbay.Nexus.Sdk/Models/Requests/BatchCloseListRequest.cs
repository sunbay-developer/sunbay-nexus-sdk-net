using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Batch close list request.
    /// Query closed (settled) batch records. You can filter results by payment channel
    /// and time range. If no time range is specified, the API returns data from the last
    /// 7 days by default. The maximum query span is 30 days.
    /// </summary>
    public class BatchCloseListRequest
    {
        /// <summary>
        /// Application ID
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// Merchant ID
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// Payment terminal serial number. The payment terminal device serial number provided by SUNBAY
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string TerminalSn { get; set; } = string.Empty;

        /// <summary>
        /// Payment channel code. If specified, only returns batches for this channel
        /// </summary>
        [JsonPropertyName("channelCode")]
        public string? ChannelCode { get; set; }

        /// <summary>
        /// Query start time, ISO 8601 format.
        /// startTime and endTime must both be present.
        /// The time span cannot exceed 30 days.
        /// If not specified, defaults to the last 7 days
        /// </summary>
        [JsonPropertyName("startTime")]
        public string? StartTime { get; set; }

        /// <summary>
        /// Query end time, ISO 8601 format.
        /// startTime and endTime must both be present.
        /// The time span cannot exceed 30 days.
        /// If not specified, defaults to the last 7 days
        /// </summary>
        [JsonPropertyName("endTime")]
        public string? EndTime { get; set; }
    }
}
