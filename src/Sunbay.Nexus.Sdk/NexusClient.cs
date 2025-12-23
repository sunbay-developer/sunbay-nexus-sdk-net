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
#if NETSTANDARD2_0
            if (options == null)
                throw new ArgumentNullException(nameof(options));
#else
            ArgumentNullException.ThrowIfNull(options);
#endif
            
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
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<SaleRequest, SaleResponse>(
                ApiConstants.PATH_SALE,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Authorization (pre-auth)
        /// </summary>
        /// <param name="request">Auth request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Auth response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<AuthResponse> AuthAsync(
            AuthRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<AuthRequest, AuthResponse>(
                ApiConstants.PATH_AUTH,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Forced authorization
        /// </summary>
        /// <param name="request">Forced auth request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Forced auth response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<ForcedAuthResponse> ForcedAuthAsync(
            ForcedAuthRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<ForcedAuthRequest, ForcedAuthResponse>(
                ApiConstants.PATH_FORCED_AUTH,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Incremental authorization
        /// </summary>
        /// <param name="request">Incremental auth request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Incremental auth response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<IncrementalAuthResponse> IncrementalAuthAsync(
            IncrementalAuthRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<IncrementalAuthRequest, IncrementalAuthResponse>(
                ApiConstants.PATH_INCREMENTAL_AUTH,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Post authorization (pre-auth completion)
        /// </summary>
        /// <param name="request">Post auth request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Post auth response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<PostAuthResponse> PostAuthAsync(
            PostAuthRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<PostAuthRequest, PostAuthResponse>(
                ApiConstants.PATH_POST_AUTH,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Refund
        /// </summary>
        /// <param name="request">Refund request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Refund response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<RefundResponse> RefundAsync(
            RefundRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<RefundRequest, RefundResponse>(
                ApiConstants.PATH_REFUND,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Void transaction
        /// </summary>
        /// <param name="request">Void request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Void response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<VoidResponse> VoidAsync(
            VoidRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<VoidRequest, VoidResponse>(
                ApiConstants.PATH_VOID,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Abort transaction
        /// </summary>
        /// <param name="request">Abort request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Abort response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<AbortResponse> AbortAsync(
            AbortRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<AbortRequest, AbortResponse>(
                ApiConstants.PATH_ABORT,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Tip adjust
        /// </summary>
        /// <param name="request">Tip adjust request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tip adjust response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<TipAdjustResponse> TipAdjustAsync(
            TipAdjustRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<TipAdjustRequest, TipAdjustResponse>(
                ApiConstants.PATH_TIP_ADJUST,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Query transaction
        /// </summary>
        /// <param name="request">Query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Query response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<QueryResponse> QueryAsync(
            QueryRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.GetAsync<QueryRequest, QueryResponse>(
                ApiConstants.PATH_QUERY,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Batch close
        /// Note: This API is currently under development.
        /// </summary>
        /// <param name="request">Batch close request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Batch close response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<BatchCloseResponse> BatchCloseAsync(
            BatchCloseRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif
            
            return await _httpClient.PostAsync<BatchCloseRequest, BatchCloseResponse>(
                ApiConstants.PATH_BATCH_CLOSE,
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
