using System;
using Sunbay.Nexus.Sdk.Constants;

namespace Sunbay.Nexus.Sdk
{
    /// <summary>
    /// Configuration options for NexusClient
    /// </summary>
    public class NexusClientOptions
    {
        /// <summary>
        /// API Key (required)
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;
        
        /// <summary>
        /// Base URL for API endpoints
        /// Default: https://open.sunbay.us
        /// </summary>
        public string BaseUrl { get; set; } = ApiConstants.DEFAULT_BASE_URL;
        
        /// <summary>
        /// Request timeout
        /// Default: 30 seconds
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(ApiConstants.DEFAULT_TIMEOUT_SECONDS);
        
        /// <summary>
        /// Maximum retry attempts for GET requests
        /// Default: 3
        /// </summary>
        public int MaxRetries { get; set; } = ApiConstants.DEFAULT_MAX_RETRIES;
        
        /// <summary>
        /// Maximum total connections in the connection pool
        /// Default: 200
        /// </summary>
        public int MaxTotalConnections { get; set; } = ApiConstants.DEFAULT_MAX_TOTAL_CONNECTIONS;
        
        /// <summary>
        /// Maximum connections per endpoint
        /// Default: 20
        /// </summary>
        public int MaxConnectionsPerEndpoint { get; set; } = ApiConstants.DEFAULT_MAX_CONNECTIONS_PER_ENDPOINT;
    }
}
