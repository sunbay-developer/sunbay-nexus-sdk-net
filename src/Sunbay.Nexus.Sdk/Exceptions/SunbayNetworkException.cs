using System;
using Sunbay.Nexus.Sdk.Constants;

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Exception for network-related errors
    /// </summary>
    public class SunbayNetworkException : SunbayException
    {
        /// <summary>
        /// Indicates whether the operation can be retried
        /// </summary>
        public bool IsRetryable { get; }
        
        /// <summary>
        /// Initializes a new instance of SunbayNetworkException
        /// </summary>
        public SunbayNetworkException(string message, bool isRetryable = true)
            : base(message, code: ApiConstants.ERROR_CODE_NETWORK_ERROR)
        {
            IsRetryable = isRetryable;
        }
        
        /// <summary>
        /// Initializes a new instance of SunbayNetworkException with inner exception
        /// </summary>
        public SunbayNetworkException(string message, Exception innerException, bool isRetryable = true)
            : base(message, innerException, code: ApiConstants.ERROR_CODE_NETWORK_ERROR)
        {
            IsRetryable = isRetryable;
        }
    }
}
