using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Create Hosted Payment Page checkout session (<c>POST /v1/checkout/create-session</c>).
    /// </summary>
    public class CreateCheckoutSessionRequest
    {
        /// <summary>
        /// Application ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// Unique transaction request ID for idempotency
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;

        /// <summary>
        /// Reference order ID (6–32 characters)
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string ReferenceOrderId { get; set; } = string.Empty;

        /// <summary>
        /// Merchant ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// Amount breakdown
        /// </summary>
        [JsonPropertyName("amount")]
        public CheckoutAmount Amount { get; set; } = new();

        /// <summary>
        /// Order description shown on checkout
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Cart items. If sent, sum of amount × num must equal amount.orderAmount.
        /// </summary>
        [JsonPropertyName("productList")]
        public List<CheckoutProductItem>? ProductList { get; set; }

        /// <summary>
        /// Whether to collect billing address on checkout. Default: false.
        /// </summary>
        [JsonPropertyName("collectBillingAddress")]
        public bool CollectBillingAddress { get; set; }

        /// <summary>
        /// Whether to collect shipping address on checkout. Default: false.
        /// </summary>
        [JsonPropertyName("collectShippingAddress")]
        public bool CollectShippingAddress { get; set; }

        /// <summary>
        /// Merchant return URL after payment completes or is cancelled
        /// </summary>
        [JsonPropertyName("merchantReturnUrl")]
        public string? MerchantReturnUrl { get; set; }

        /// <summary>
        /// Webhook URL for async payment notifications (public HTTPS)
        /// </summary>
        [JsonPropertyName("notifyUrl")]
        public string? NotifyUrl { get; set; }
    }
}
