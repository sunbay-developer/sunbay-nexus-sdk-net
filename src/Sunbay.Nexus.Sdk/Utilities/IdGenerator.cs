using System;

namespace Sunbay.Nexus.Sdk.Utilities
{
    /// <summary>
    /// ID generator utility
    /// </summary>
    internal static class IdGenerator
    {
        /// <summary>
        /// Generate UUID
        /// </summary>
        /// <returns>UUID string</returns>
        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Generate request ID
        /// </summary>
        /// <returns>Request ID</returns>
        public static string GenerateRequestId()
        {
            return GenerateUuid();
        }
    }
}

