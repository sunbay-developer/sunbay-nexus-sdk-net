using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Batch query response
    /// </summary>
    public class BatchQueryResponse : BaseResponse
    {
        /// <summary>
        /// Batch list, statistics grouped by channel code and price currency
        /// </summary>
        [JsonPropertyName("batchList")]
        public List<BatchQueryItem>? BatchList { get; set; }
    }
}
