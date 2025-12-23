using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Query request
    /// </summary>
    public class QueryRequest
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
        /// SUNBAY Nexus transaction ID
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }
        
        /// <summary>
        /// Transaction request ID
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string? TransactionRequestId { get; set; }
    }
}
