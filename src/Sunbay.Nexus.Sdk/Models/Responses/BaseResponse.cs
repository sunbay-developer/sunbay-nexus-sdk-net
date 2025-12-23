using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Base response for all API responses
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// Response code, "0" means success
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        
        /// <summary>
        /// Response message
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Trace ID for troubleshooting
        /// </summary>
        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }
        
        /// <summary>
        /// Check if response is successful
        /// </summary>
        [JsonIgnore]
        public bool Success => Code == "0";
    }
}
