using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Abort response
    /// </summary>
    public class AbortResponse : BaseResponse
    {
        /// <summary>
        /// Aborted transaction's SUNBAY Nexus transaction ID
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public string? OriginalTransactionId { get; set; }
        
        /// <summary>
        /// Aborted transaction's request ID (only returned when provided in request)
        /// </summary>
        [JsonPropertyName("originalTransactionRequestId")]
        public string? OriginalTransactionRequestId { get; set; }
    }
}

