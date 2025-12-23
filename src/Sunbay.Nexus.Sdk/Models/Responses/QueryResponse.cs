using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Query response
    /// </summary>
    public class QueryResponse : BaseResponse
    {
        /// <summary>
        /// SUNBAY Nexus transaction ID
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Transaction request ID
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
        
        /// <summary>
        /// Reference order ID
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string? ReferenceOrderId { get; set; }
        
        /// <summary>
        /// Transaction status
        /// </summary>
        [JsonPropertyName("transactionStatus")]
        public string? TransactionStatus { get; set; }
        
        /// <summary>
        /// Transaction type
        /// </summary>
        [JsonPropertyName("transactionType")]
        public string? TransactionType { get; set; }
        
        /// <summary>
        /// Transaction amount details
        /// </summary>
        [JsonPropertyName("amount")]
        public Amount? Amount { get; set; }
        
        /// <summary>
        /// Transaction creation time
        /// </summary>
        [JsonPropertyName("createTime")]
        public string? CreateTime { get; set; }
        
        /// <summary>
        /// Transaction completion time
        /// </summary>
        [JsonPropertyName("completeTime")]
        public string? CompleteTime { get; set; }
        
        /// <summary>
        /// Masked card number
        /// </summary>
        [JsonPropertyName("maskedPan")]
        public string? MaskedPan { get; set; }
        
        /// <summary>
        /// Card network type
        /// </summary>
        [JsonPropertyName("cardNetworkType")]
        public string? CardNetworkType { get; set; }
        
        /// <summary>
        /// Payment method ID
        /// </summary>
        [JsonPropertyName("paymentMethodId")]
        public string? PaymentMethodId { get; set; }
        
        /// <summary>
        /// Sub payment method ID
        /// </summary>
        [JsonPropertyName("subPaymentMethodId")]
        public string? SubPaymentMethodId { get; set; }
        
        /// <summary>
        /// Batch number
        /// </summary>
        [JsonPropertyName("batchNo")]
        public string? BatchNo { get; set; }
        
        /// <summary>
        /// Voucher number
        /// </summary>
        [JsonPropertyName("voucherNo")]
        public string? VoucherNo { get; set; }
        
        /// <summary>
        /// System trace number
        /// </summary>
        [JsonPropertyName("stan")]
        public string? Stan { get; set; }
        
        /// <summary>
        /// Reference number
        /// </summary>
        [JsonPropertyName("rrn")]
        public string? Rrn { get; set; }
        
        /// <summary>
        /// Authorization code
        /// </summary>
        [JsonPropertyName("authCode")]
        public string? AuthCode { get; set; }
        
        /// <summary>
        /// Entry mode
        /// </summary>
        [JsonPropertyName("entryMode")]
        public string? EntryMode { get; set; }
        
        /// <summary>
        /// Authentication method
        /// </summary>
        [JsonPropertyName("authenticationMethod")]
        public string? AuthenticationMethod { get; set; }
        
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
        /// Terminal serial number
        /// </summary>
        [JsonPropertyName("terminalSn")]
        public string? TerminalSn { get; set; }
        
        /// <summary>
        /// Product description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Additional data
        /// </summary>
        [JsonPropertyName("attach")]
        public string? Attach { get; set; }
    }
}
