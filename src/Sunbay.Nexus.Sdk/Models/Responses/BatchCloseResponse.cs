using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Batch close response
    /// </summary>
    public class BatchCloseResponse : BaseResponse
    {
        /// <summary>
        /// Batch number
        /// </summary>
        [JsonPropertyName("batchNo")]
        public string? BatchNo { get; set; }
        
        /// <summary>
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Batch close time, format: yyyy-MM-DDTHH:mm:ss+TIMEZONE (ISO 8601)
        /// </summary>
        [JsonPropertyName("closeTime")]
        public string? CloseTime { get; set; }
        
        /// <summary>
        /// Number of transactions in the batch
        /// </summary>
        [JsonPropertyName("transactionCount")]
        public int? TransactionCount { get; set; }
        
        /// <summary>
        /// Total amount of the batch
        /// </summary>
        [JsonPropertyName("totalAmount")]
        public BatchTotalAmount? TotalAmount { get; set; }
    }
}

