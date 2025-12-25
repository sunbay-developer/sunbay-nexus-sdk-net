using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Transaction type enum
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Sale transaction
        /// </summary>
        [EnumMember(Value = "SALE")]
        Sale,
        
        /// <summary>
        /// Authorization (pre-auth)
        /// </summary>
        [EnumMember(Value = "AUTH")]
        Auth,
        
        /// <summary>
        /// Forced authorization
        /// </summary>
        [EnumMember(Value = "FORCED_AUTH")]
        ForcedAuth,
        
        /// <summary>
        /// Incremental authorization
        /// </summary>
        [EnumMember(Value = "INCREMENTAL")]
        Incremental,
        
        /// <summary>
        /// Post authorization (pre-auth completion)
        /// </summary>
        [EnumMember(Value = "POST_AUTH")]
        PostAuth,
        
        /// <summary>
        /// Refund
        /// </summary>
        [EnumMember(Value = "REFUND")]
        Refund,
        
        /// <summary>
        /// Void
        /// </summary>
        [EnumMember(Value = "VOID")]
        Void
    }
}

