using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Payment method for online direct payment (wallet). Use with <c>cardEncryptedData</c> from the wallet.
    /// </summary>
    public enum OnlineCheckoutPaymentMethod
    {
        /// <summary>
        /// Google Pay
        /// </summary>
        [EnumMember(Value = "GOOGLE_PAY")]
        GooglePay,

        /// <summary>
        /// Apple Pay
        /// </summary>
        [EnumMember(Value = "APPLE_PAY")]
        ApplePay
    }
}
