using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Merchant terminals query response
    /// </summary>
    public class MerchantTerminalsQueryResponse : BaseResponse
    {
        /// <summary>
        /// Merchant ID (echoed from the request)
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// Opaque pagination token for retrieving the next page.
        /// Only present when more terminals are available; absence indicates the end of the list.
        /// </summary>
        [JsonPropertyName("nextToken")]
        public string? NextToken { get; set; }

        /// <summary>
        /// Terminals on the current page
        /// </summary>
        [JsonPropertyName("terminals")]
        public List<TerminalItem>? Terminals { get; set; }
    }
}
