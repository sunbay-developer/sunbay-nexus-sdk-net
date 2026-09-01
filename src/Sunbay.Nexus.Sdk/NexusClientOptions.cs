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
        public string BaseUrl { get; set; } = ApiConstants.DefaultBaseUrl;
        
        /// <summary>
        /// Request timeout
        /// Default: 30 seconds
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(ApiConstants.DefaultTimeoutSeconds);
        
        /// <summary>
        /// Maximum retry attempts for GET requests
        /// Default: 3
        /// </summary>
        public int MaxRetries { get; set; } = ApiConstants.DefaultMaxRetries;
        
        /// <summary>
        /// Maximum total connections in the connection pool.
        /// <para>
        /// NOTE: On the current .NET HTTP stack (both <c>HttpClientHandler</c> and
        /// <c>SocketsHttpHandler</c>) only a per-endpoint limit is enforced; there is no
        /// global total-connection cap. This value is kept for API compatibility and future
        /// use but is currently NOT applied. Tune <see cref="MaxConnectionsPerEndpoint"/> instead.
        /// </para>
        /// Default: 200
        /// </summary>
        public int MaxTotalConnections { get; set; } = ApiConstants.DefaultMaxTotalConnections;
        
        /// <summary>
        /// Maximum connections per endpoint
        /// Default: 200
        /// </summary>
        public int MaxConnectionsPerEndpoint { get; set; } = ApiConstants.DefaultMaxConnectionsPerEndpoint;

        /// <summary>
        /// Connect timeout for establishing a new TCP/TLS connection.
        /// Independent from the overall request <see cref="Timeout"/> so a slow-connecting
        /// host fails fast without waiting for the full request timeout.
        /// <para>NOTE: Only honored on .NET 6+ (uses SocketsHttpHandler). Ignored on .NET Standard 2.0.</para>
        /// Default: 10 seconds
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(ApiConstants.DefaultConnectTimeoutSeconds);

        /// <summary>
        /// Maximum lifetime of a pooled connection before it is recycled.
        /// Ensures DNS changes and load-balancer rotations are eventually picked up.
        /// <para>NOTE: Only honored on .NET 6+ (uses SocketsHttpHandler). Ignored on .NET Standard 2.0.</para>
        /// Default: 5 minutes. Use <see cref="TimeSpan.Zero"/> or negative to disable recycling by lifetime.
        /// </summary>
        public TimeSpan PooledConnectionLifetime { get; set; } = TimeSpan.FromSeconds(ApiConstants.DefaultPooledConnectionLifetimeSeconds);

        /// <summary>
        /// Maximum time a pooled connection may remain idle before it is closed.
        /// Prevents reusing dead connections silently killed by NAT/proxies.
        /// <para>NOTE: Only honored on .NET 6+ (uses SocketsHttpHandler). Ignored on .NET Standard 2.0.</para>
        /// Default: 2 minutes.
        /// </summary>
        public TimeSpan PooledConnectionIdleTimeout { get; set; } = TimeSpan.FromSeconds(ApiConstants.DefaultPooledConnectionIdleTimeoutSeconds);
    }
}
