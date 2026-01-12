using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Statistics grouped by channel code and price currency
    /// </summary>
    public class BatchQueryItem
    {
        /// <summary>
        /// Batch number
        /// </summary>
        [JsonPropertyName("batchNo")]
        public string? BatchNo { get; set; }

        /// <summary>
        /// Batch start time, format: yyyy-MM-DDTHH:mm:ss+TIMEZONE (ISO 8601)
        /// </summary>
        [JsonPropertyName("startTime")]
        public string? StartTime { get; set; }

        /// <summary>
        /// Channel code
        /// </summary>
        [JsonPropertyName("channelCode")]
        public string? ChannelCode { get; set; }

        /// <summary>
        /// Price currency (ISO 4217)
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceCurrency { get; set; }

        /// <summary>
        /// Total number of transactions
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Net amount, using smallest currency unit (minor units)
        /// </summary>
        [JsonPropertyName("netAmount")]
        public long? NetAmount { get; set; }

        /// <summary>
        /// Tip amount, using smallest currency unit (minor units)
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long? TipAmount { get; set; }

        /// <summary>
        /// Surcharge amount, using smallest currency unit (minor units)
        /// </summary>
        [JsonPropertyName("surchargeAmount")]
        public long? SurchargeAmount { get; set; }

        /// <summary>
        /// Tax amount, using smallest currency unit (minor units)
        /// </summary>
        [JsonPropertyName("taxAmount")]
        public long? TaxAmount { get; set; }
    }
}
