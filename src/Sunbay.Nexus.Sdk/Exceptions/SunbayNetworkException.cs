using System;
using Sunbay.Nexus.Sdk.Constants;
#if NETSTANDARD2_0
using System.Runtime.Serialization;
#endif

namespace Sunbay.Nexus.Sdk.Exceptions
{
    /// <summary>
    /// Exception for network-related errors
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class SunbayNetworkException : SunbayException
    {
        /// <summary>
        /// Indicates whether the operation can be retried
        /// </summary>
        public bool IsRetryable { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="SunbayNetworkException"/>.
        /// </summary>
        public SunbayNetworkException(string message, bool isRetryable = true)
            : base(message, code: ApiConstants.ErrorCodeNetworkError)
        {
            IsRetryable = isRetryable;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SunbayNetworkException"/> with inner exception.
        /// </summary>
        public SunbayNetworkException(string message, Exception innerException, bool isRetryable = true)
            : base(message, innerException, code: ApiConstants.ErrorCodeNetworkError)
        {
            IsRetryable = isRetryable;
        }

#if NETSTANDARD2_0
        /// <summary>Serialization constructor.</summary>
        protected SunbayNetworkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            IsRetryable = info.GetBoolean(nameof(IsRetryable));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
            info.AddValue(nameof(IsRetryable), IsRetryable);
        }
#endif
    }
}
