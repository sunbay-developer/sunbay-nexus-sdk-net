using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Tip adjust response
    /// </summary>
    public class TipAdjustResponse : BaseResponse
    {
        /// <summary>
        /// Original transaction's SUNBAY Nexus transaction ID (only returned when provided in request)
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Original transaction's request ID (only returned when provided in request)
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
        
        /// <summary>
        /// Adjusted tip amount, in smallest currency unit (e.g., cents for USD, fen for CNY), returned as-is from request
        /// </summary>
        [JsonPropertyName("tipAmount")]
        public long? TipAmount { get; set; }
    }
}

