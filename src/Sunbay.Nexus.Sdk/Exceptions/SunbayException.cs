using System;

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Base exception for Sunbay SDK
    /// </summary>
    public class SunbayException : Exception
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string? Code { get; }
        
        /// <summary>
        /// Trace ID for debugging
        /// </summary>
        public string? TraceId { get; }
        
        /// <summary>
        /// Initializes a new instance of SunbayException
        /// </summary>
        public SunbayException(string message, string? code = null, string? traceId = null)
            : base(message)
        {
            Code = code;
            TraceId = traceId;
        }
        
        /// <summary>
        /// Initializes a new instance of SunbayException with inner exception
        /// </summary>
        public SunbayException(string message, Exception innerException, string? code = null, string? traceId = null)
            : base(message, innerException)
        {
            Code = code;
            TraceId = traceId;
        }
    }
}
