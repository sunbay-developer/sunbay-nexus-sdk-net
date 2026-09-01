using System;
#if NETSTANDARD2_0
using System.Runtime.Serialization;
#endif

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Base exception for Sunbay SDK
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
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
        /// Initializes a new instance of <see cref="SunbayException"/>.
        /// </summary>
        public SunbayException(string message, string? code = null, string? traceId = null)
            : base(message)
        {
            Code = code;
            TraceId = traceId;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SunbayException"/> with inner exception.
        /// </summary>
        public SunbayException(string message, Exception innerException, string? code = null, string? traceId = null)
            : base(message, innerException)
        {
            Code = code;
            TraceId = traceId;
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected SunbayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = info.GetString(nameof(Code));
            TraceId = info.GetString(nameof(TraceId));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(TraceId), TraceId);
        }
#endif
    }
}
