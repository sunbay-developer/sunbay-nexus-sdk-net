using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Batch close list response
    /// </summary>
    public class BatchCloseListResponse : BaseResponse
    {
        /// <summary>
        /// List of closed batch records
        /// </summary>
        [JsonPropertyName("batchCloseList")]
        public List<BatchCloseListItem>? BatchCloseList { get; set; }
    }
}
