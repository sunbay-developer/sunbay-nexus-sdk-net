using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk.Constants;
using Sunbay.Nexus.Sdk.Exceptions;
using Sunbay.Nexus.Sdk.Http;
using Sunbay.Nexus.Sdk.Models.Requests;
using Sunbay.Nexus.Sdk.Models.Responses;

namespace Sunbay.Nexus.Sdk
{
    /// <summary>
    /// Sunbay Nexus API client
    /// This client is thread-safe and can be safely used by multiple threads.
    /// </summary>
    public class NexusClient : IAsyncDisposable
    {
        private readonly HttpClientWrapper _httpClient;
        private bool _disposed;
        
        /// <summary>
        /// Initializes a new instance of NexusClient
        /// </summary>
        /// <param name="options">Client configuration options</param>
        /// <param name="loggerFactory">Optional logger factory instance</param>
        /// <exception cref="ArgumentNullException">Thrown when options is null</exception>
        /// <exception cref="SunbayBusinessException">Thrown when API key is invalid</exception>
        public NexusClient(NexusClientOptions options, ILoggerFactory? loggerFactory = null)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            
            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                throw new SunbayBusinessException(
                    ApiConstants.ERROR_CODE_PARAMETER_ERROR,
                    ApiConstants.MESSAGE_API_KEY_REQUIRED);
            }
            
            // Create logger for HttpClientWrapper with specific category
            var httpLogger = loggerFactory?.CreateLogger("Sunbay.Nexus.Sdk.Http.HttpClientWrapper");
            _httpClient = new HttpClientWrapper(options, httpLogger);
        }
        
        /// <summary>
        /// Execute a sale transaction
        /// </summary>
        /// <param name="request">Sale request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Sale response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<SaleResponse> SaleAsync(
            SaleRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            return await _httpClient.PostAsync<SaleRequest, SaleResponse>(
                ApiConstants.PATH_SALE,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Dispose resources asynchronously
        /// </summary>
        public ValueTask DisposeAsync()
        {
            if (_disposed)
#if NETSTANDARD2_0
                return new ValueTask(Task.CompletedTask);
#else
                return ValueTask.CompletedTask;
#endif
            
            _httpClient?.Dispose();
            _disposed = true;
            
#if NETSTANDARD2_0
            return new ValueTask(Task.CompletedTask);
#else
            return ValueTask.CompletedTask;
#endif
        }
    }
}
