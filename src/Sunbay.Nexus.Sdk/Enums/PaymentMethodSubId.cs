using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Sub payment method (subId). When paymentMethod.category is EBT and id is EBT, subId can be specified; when category is CARD, subId must not be specified.
    /// </summary>
    public enum PaymentMethodSubId
    {
        /// <summary>
        /// SNAP (Supplemental Nutrition Assistance Program)
        /// </summary>
        [EnumMember(Value = "SNAP")]
        Snap,

        /// <summary>
        /// VOUCHER
        /// </summary>
        [EnumMember(Value = "VOUCHER")]
        Voucher,

        /// <summary>
        /// BENEFIT
        /// </summary>
        [EnumMember(Value = "BENEFIT")]
        Benefit
    }
}
