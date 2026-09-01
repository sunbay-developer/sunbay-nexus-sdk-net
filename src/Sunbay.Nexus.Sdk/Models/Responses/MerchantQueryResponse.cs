using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Responses
{
    /// <summary>
    /// Merchant query response
    /// </summary>
    public class MerchantQueryResponse : BaseResponse
    {
        /// <summary>
        /// Merchant ID
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// "Doing Business As" name — the merchant's public/trading name shown to customers
        /// </summary>
        [JsonPropertyName("dbaName")]
        public string? DbaName { get; set; }

        /// <summary>
        /// Merchant Category Code (ISO 18245)
        /// </summary>
        [JsonPropertyName("mcc")]
        public string? Mcc { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-3 country code
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// State or province name
        /// </summary>
        [JsonPropertyName("stateName")]
        public string? StateName { get; set; }

        /// <summary>
        /// City name
        /// </summary>
        [JsonPropertyName("cityName")]
        public string? CityName { get; set; }

        /// <summary>
        /// Street address
        /// </summary>
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        /// <summary>
        /// Full detailed address (street number, suite, etc.)
        /// </summary>
        [JsonPropertyName("detailAddress")]
        public string? DetailAddress { get; set; }

        /// <summary>
        /// Postal / ZIP code
        /// </summary>
        [JsonPropertyName("zipCode")]
        public string? ZipCode { get; set; }

        /// <summary>
        /// Merchant status. Y: active, N: inactive
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Merchant creation time (ISO 8601)
        /// </summary>
        [JsonPropertyName("createTime")]
        public string? CreateTime { get; set; }

        /// <summary>
        /// MIDs assigned to this merchant by each payment channel (Processor).
        /// A merchant may be onboarded to multiple processors, and each processor assigns its own MID.
        /// Returns an empty array if no payment channel has been enabled yet.
        /// </summary>
        [JsonPropertyName("midList")]
        public List<MerchantMidItem>? MidList { get; set; }
    }
}
