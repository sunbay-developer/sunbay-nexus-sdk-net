using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Online refund request (POST /v1/checkout/refund).
    /// Either <see cref="OriginalTransactionId"/> or <see cref="OriginalTransactionRequestId"/> must be provided
    /// to identify the original transaction to refund.
    /// </summary>
    public class OnlineRefundRequest
    {
        /// <summary>
        /// Application ID
        /// </summary>
        [JsonPropertyName("appId")]
        public string? AppId { get; set; }

        /// <summary>
        /// Merchant ID
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// Transaction request ID for this refund transaction.
        /// Unique ID to identify this refund request, used as API idempotency control field.
        /// Only letters, numbers, underscores and hyphens are supported, max length 64.
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }

        /// <summary>
        /// Original transaction ID to refund (SUNBAY transaction ID from the payment response).
        /// Either originalTransactionId or originalTransactionRequestId is required.
        /// If both are provided, originalTransactionId takes priority.
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }

        /// <summary>
        /// Original transaction request ID to refund.
        /// Either originalTransactionId or originalTransactionRequestId is required.
        /// If both are provided, originalTransactionId takes priority.
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }

        /// <summary>
        /// Refund amount information.
        /// If totalAmount is provided, system will validate it equals orderAmount + taxAmount + surchargeAmount + tipAmount.
        /// </summary>
        [JsonPropertyName("amount")]
        public OnlineRefundAmount? Amount { get; set; }

        /// <summary>
        /// Refund description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Additional data, returned as-is, can be used to record refund reason or other custom information
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }

        /// <summary>
        /// Asynchronous notification URL (Webhook). Must be a publicly accessible HTTPS address if provided.
        /// </summary>
        [JsonPropertyName("notifyUrl")]
        public string? NotifyUrl { get; set; }
    }
}
