using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Online refund response (POST /v1/checkout/refund).
    /// </summary>
    public class OnlineRefundResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID for this refund transaction
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Transaction request ID, returned as-is from request
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }

        /// <summary>
        /// Original transaction ID
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }

        /// <summary>
        /// Transaction status: INITIAL(I)/PROCESSING(P)/SUCCESS(S)/FAIL(F)/CLOSED(C)
        /// </summary>
        [JsonPropertyName("transactionStatus")]
        public string? TransactionStatus { get; set; }

        /// <summary>
        /// Transaction type, fixed as REFUND
        /// </summary>
        [JsonPropertyName("transactionType")]
        public string? TransactionType { get; set; }

        /// <summary>
        /// Refund amount information (smallest currency unit)
        /// </summary>
        [JsonPropertyName("amount")]
        public OnlineRefundAmount? Amount { get; set; }

        /// <summary>
        /// Refund creation time, ISO 8601 format
        /// </summary>
        [JsonPropertyName("createTime")]
        public string? CreateTime { get; set; }

        /// <summary>
        /// Refund completion time, returned when transaction reaches terminal state (S/F). ISO 8601 format
        /// </summary>
        [JsonPropertyName("completeTime")]
        public string? CompleteTime { get; set; }

        /// <summary>
        /// Transaction result code
        /// </summary>
        [JsonPropertyName("transactionResultCode")]
        public string? TransactionResultCode { get; set; }

        /// <summary>
        /// Transaction result message
        /// </summary>
        [JsonPropertyName("transactionResultMsg")]
        public string? TransactionResultMsg { get; set; }

        /// <summary>
        /// Refund description (returned as-is from request)
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
