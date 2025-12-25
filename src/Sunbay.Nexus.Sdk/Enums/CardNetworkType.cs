using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Card network type enum
    /// </summary>
    public enum CardNetworkType
    {
        /// <summary>
        /// Credit card
        /// </summary>
        [EnumMember(Value = "CREDIT")]
        Credit,
        
        /// <summary>
        /// Debit card
        /// </summary>
        [EnumMember(Value = "DEBIT")]
        Debit,
        
        /// <summary>
        /// EBT (Electronic Benefit Transfer)
        /// </summary>
        [EnumMember(Value = "EBT")]
        Ebt,
        
        /// <summary>
        /// EGC (Electronic Gift Card)
        /// </summary>
        [EnumMember(Value = "EGC")]
        Egc,
        
        /// <summary>
        /// Unknown card type
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown
    }
}

