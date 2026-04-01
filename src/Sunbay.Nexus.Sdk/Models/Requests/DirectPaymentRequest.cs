using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sunbay.Nexus.Sdk.Enums;
using Sunbay.Nexus.Sdk.Models.Common;

namespace Sunbay.Nexus.Sdk.Models.Requests
{
    /// <summary>
    /// Online direct payment without Hosted Payment Page session (<c>POST /v1/checkout/sale</c>).
    /// Supports digital wallets (e.g. Google Pay, Apple Pay) with encrypted payload from the wallet.
    /// </summary>
    public class DirectPaymentRequest
    {
        /// <summary>
        /// Application ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// Merchant ID assigned by SUNBAY
        /// </summary>
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// Unique request ID for idempotency
        /// </summary>
        [JsonPropertyName("transactionRequestId")]
        public string TransactionRequestId { get; set; } = string.Empty;

        /// <summary>
        /// Merchant order ID (6–32 characters)
        /// </summary>
        [JsonPropertyName("referenceOrderId")]
        public string ReferenceOrderId { get; set; } = string.Empty;

        /// <summary>
        /// Order description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount breakdown
        /// </summary>
        [JsonPropertyName("amount")]
        public CheckoutAmount Amount { get; set; } = new();

        /// <summary>
        /// Product list items. If sent, sum of amount × num must equal amount.orderAmount.
        /// </summary>
        [JsonPropertyName("productList")]
        public List<CheckoutProductItem>? ProductList { get; set; }

        /// <summary>
        /// Payment method (wallet). Required with <c>cardEncryptedData</c> for wallet flows.
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        public OnlineCheckoutPaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Digital wallet encrypted token (JSON string). Required when payment method is Google Pay or Apple Pay.
        /// </summary>
        [JsonPropertyName("cardEncryptedData")]
        public string? CardEncryptedData { get; set; }

        /// <summary>
        /// Buyer email
        /// </summary>
        [JsonPropertyName("customerEmail")]
        public string? CustomerEmail { get; set; }

        /// <summary>
        /// Buyer name
        /// </summary>
        [JsonPropertyName("customerName")]
        public string? CustomerName { get; set; }

        /// <summary>
        /// Billing address
        /// </summary>
        [JsonPropertyName("billingAddress")]
        public CheckoutAddress? BillingAddress { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        [JsonPropertyName("shippingAddress")]
        public CheckoutAddress? ShippingAddress { get; set; }

        /// <summary>
        /// Webhook URL for async payment results (public HTTPS)
        /// </summary>
        [JsonPropertyName("notifyUrl")]
        public string? NotifyUrl { get; set; }

        /// <summary>
        /// Browser return URL after payment (e.g. 3DS redirect flow)
        /// </summary>
        [JsonPropertyName("merchantReturnUrl")]
        public string? MerchantReturnUrl { get; set; }
    }
}
