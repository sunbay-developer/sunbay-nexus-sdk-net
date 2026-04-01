using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Product entry in checkout <c>productList</c>.
    /// If sent, the sum of each item's amount × num must equal amount.orderAmount.
    /// </summary>
    public class CheckoutProductItem
    {
        /// <summary>
        /// Item amount in smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Quantity.
        /// </summary>
        [JsonPropertyName("num")]
        public int Num { get; set; }
    }
}
