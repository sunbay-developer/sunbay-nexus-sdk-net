using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Transaction status enum
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Initial state
        /// </summary>
        [EnumMember(Value = "I")]
        Initial,
        
        /// <summary>
        /// Transaction processing
        /// </summary>
        [EnumMember(Value = "P")]
        Processing,
        
        /// <summary>
        /// Transaction successful
        /// </summary>
        [EnumMember(Value = "S")]
        Success,
        
        /// <summary>
        /// Transaction failed
        /// </summary>
        [EnumMember(Value = "F")]
        Fail,
        
        /// <summary>
        /// Transaction closed
        /// </summary>
        [EnumMember(Value = "C")]
        Closed
    }
}

