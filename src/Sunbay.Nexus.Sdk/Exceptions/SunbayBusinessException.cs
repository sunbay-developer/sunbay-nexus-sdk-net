using System;
#if NETSTANDARD2_0
using System.Runtime.Serialization;
#endif

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Exception for business logic errors
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class SunbayBusinessException : SunbayException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SunbayBusinessException"/>.
        /// </summary>
        public SunbayBusinessException(string code, string message, string? traceId = null)
            : base(message, code, traceId)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SunbayBusinessException"/> with inner exception.
        /// </summary>
        public SunbayBusinessException(string code, string message, Exception innerException, string? traceId = null)
            : base(message, innerException, code, traceId)
        {
        }

#if NETSTANDARD2_0
        /// <summary>Serialization constructor.</summary>
        protected SunbayBusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
