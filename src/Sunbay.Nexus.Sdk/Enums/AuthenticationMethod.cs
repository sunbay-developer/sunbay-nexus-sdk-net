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
        NotAuthenticated,
        
        /// <summary>
        /// PIN authentication
        /// </summary>
        Pin,
        
        /// <summary>
        /// Offline PIN
        /// </summary>
        OfflinePin,
        
        /// <summary>
        /// Bypass authentication
        /// </summary>
        ByPass,
        
        /// <summary>
        /// Signature authentication
        /// </summary>
        Signature
    }
}

