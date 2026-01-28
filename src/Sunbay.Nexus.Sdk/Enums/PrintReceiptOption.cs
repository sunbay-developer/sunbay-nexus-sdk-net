using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Receipt print option for transaction.
    /// </summary>
    /// <since>2025-01-28</since>
    public enum PrintReceiptOption
    {
        /// <summary>
        /// Do not print receipt
        /// </summary>
        [EnumMember(Value = "NONE")]
        None,

        /// <summary>
        /// Print merchant copy only
        /// </summary>
        [EnumMember(Value = "MERCHANT")]
        Merchant,

        /// <summary>
        /// Print customer copy only
        /// </summary>
        [EnumMember(Value = "CUSTOMER")]
        Customer,

        /// <summary>
        /// Print both merchant and customer copies
        /// </summary>
        [EnumMember(Value = "BOTH")]
        Both
    }
}
