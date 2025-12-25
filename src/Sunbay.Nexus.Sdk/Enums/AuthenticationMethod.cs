using System.Runtime.Serialization;

namespace Sunbay.Nexus.Sdk.Enums
{
    /// <summary>
    /// Authentication method enum
    /// </summary>
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Not authenticated
        /// </summary>
        [EnumMember(Value = "NOT_AUTHENTICATED")]
        NotAuthenticated,
        
        /// <summary>
        /// PIN authentication
        /// </summary>
        [EnumMember(Value = "PIN")]
        Pin,
        
        /// <summary>
        /// Offline PIN
        /// </summary>
        [EnumMember(Value = "OFFLINE_PIN")]
        OfflinePin,
        
        /// <summary>
        /// Bypass authentication
        /// </summary>
        [EnumMember(Value = "BY_PASS")]
        ByPass,
        
        /// <summary>
        /// Signature authentication
        /// </summary>
        [EnumMember(Value = "SIGNATURE")]
        Signature
    }
}

