using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Transaction batch settlement status enum.
    /// </summary>
    public enum TransactionBatchStatus
    {
        /// <summary>
        /// No batch settlement needed
        /// </summary>
        [EnumMember(Value = "NB")]
        NB,

        /// <summary>
        /// Waiting for batch close
        /// </summary>
        [EnumMember(Value = "UB")]
        UB,

        /// <summary>
        /// Batch closed
        /// </summary>
        [EnumMember(Value = "BC")]
        BC
    }
}
