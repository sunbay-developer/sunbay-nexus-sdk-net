using System;

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Exception for business logic errors
    /// </summary>
    public class SunbayBusinessException : SunbayException
    {
        /// <summary>
        /// Initializes a new instance of SunbayBusinessException
        /// </summary>
        public SunbayBusinessException(string code, string message, string? traceId = null)
            : base(message, code, traceId)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of SunbayBusinessException with inner exception
        /// </summary>
        public SunbayBusinessException(string code, string message, Exception innerException, string? traceId = null)
            : base(message, innerException, code, traceId)
        {
        }
    }
}
