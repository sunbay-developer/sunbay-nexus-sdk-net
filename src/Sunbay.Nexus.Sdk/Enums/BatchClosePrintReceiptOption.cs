using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Batch close report print option.
    /// </summary>
    public enum BatchClosePrintReceiptOption
    {
        /// <summary>
        /// Print summary report only.
        /// </summary>
        [EnumMember(Value = "TOTAL")]
        Total,

        /// <summary>
        /// Print detailed report only.
        /// </summary>
        [EnumMember(Value = "DETAIL")]
        Detail,

        /// <summary>
        /// Print both summary and detail reports.
        /// </summary>
        [EnumMember(Value = "BOTH")]
        Both,

        /// <summary>
        /// Do not print batch close report.
        /// </summary>
        [EnumMember(Value = "NONE")]
        None,

        /// <summary>
        /// Use SUNBAY platform configuration.
        /// </summary>
        [EnumMember(Value = "AUTO")]
        Auto
    }
}
