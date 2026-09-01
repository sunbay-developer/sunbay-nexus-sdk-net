using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Batch close list item information. Represents a single closed (settled) batch record.
    /// </summary>
    public class BatchCloseListItem
    {
        /// <summary>
        /// Batch number
        /// </summary>
        [JsonPropertyName("batchNo")]
        public string? BatchNo { get; set; }

        /// <summary>
        /// Batch status: S - Success
        /// </summary>
        [JsonPropertyName("batchStatus")]
        public string? BatchStatus { get; set; }

        /// <summary>
        /// Batch close time, ISO 8601 format
        /// </summary>
        [JsonPropertyName("batchTime")]
        public string? BatchTime { get; set; }

        /// <summary>
        /// Total number of transactions in the batch
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Total net amount, using minor units.
        /// The number of decimal places for each currency can refer to the ISO-4217 standard
        /// </summary>
        [JsonPropertyName("netAmount")]
        public long? NetAmount { get; set; }

        /// <summary>
        /// Transaction currency (ISO 4217, e.g. USD, CNY)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }

        /// <summary>
        /// Payment channel code
        /// </summary>
        [JsonPropertyName("channelCode")]
        public string? ChannelCode { get; set; }

        /// <summary>
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }

        /// <summary>
        /// Merchant Identification number (MID) assigned by the payment processor
        /// </summary>
        [JsonPropertyName("mid")]
        public string? Mid { get; set; }

        /// <summary>
        /// Terminal Identification number (TID) assigned by the payment processor
        /// </summary>
        [JsonPropertyName("tid")]
        public string? Tid { get; set; }
    }
}
