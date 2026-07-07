using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Related transaction status enum.
    /// Indicates the lifecycle change of the current transaction due to subsequent transactions.
    /// </summary>
    public enum RelatedTransactionStatus
    {
        /// <summary>
        /// Transaction has been voided
        /// </summary>
        [EnumMember(Value = "VOIDED")]
        Voided,

        /// <summary>
        /// Transaction has incremental authorization
        /// </summary>
        [EnumMember(Value = "INCREMENTAL")]
        Incremental,

        /// <summary>
        /// Transaction has been fully refunded
        /// </summary>
        [EnumMember(Value = "REFUNDED")]
        Refunded,

        /// <summary>
        /// Transaction has been captured (post-auth)
        /// </summary>
        [EnumMember(Value = "CAPTURE")]
        Capture,

        /// <summary>
        /// Transaction has been partially refunded
        /// </summary>
        [EnumMember(Value = "PART_REFUNDED")]
        PartRefunded
    }
}
