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
        Sale,
        
        /// <summary>
        /// Authorization (pre-auth)
        /// </summary>
        Auth,
        
        /// <summary>
        /// Forced authorization
        /// </summary>
        ForcedAuth,
        
        /// <summary>
        /// Incremental authorization
        /// </summary>
        Incremental,
        
        /// <summary>
        /// Post authorization (pre-auth completion)
        /// </summary>
        PostAuth,
        
        /// <summary>
        /// Refund
        /// </summary>
        Refund,
        
        /// <summary>
        /// Void
        /// </summary>
        Void
    }
}

