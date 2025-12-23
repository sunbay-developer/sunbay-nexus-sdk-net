using System.Text.Json.Serialization;

namespace Sunbay.Nexus.Sdk.Models.Common
{
    /// <summary>
    /// Payment method information
    /// </summary>
    public class PaymentMethodInfo
    {
        /// <summary>
        /// Payment category: CARD (bank card)/CARD-CREDIT (credit card network)/CARD-DEBIT (debit card network)/QR-MPM (QR code merchant present mode)/QR-CPM (QR code customer present mode)
        /// </summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        
        /// <summary>
        /// Specific payment method: WECHAT (WeChat)/ALIPAY (Alipay) etc. For card payments, usually only category needs to be specified
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}

