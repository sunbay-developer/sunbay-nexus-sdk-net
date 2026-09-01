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
    /// Sunbay Nexus API client.
    /// This client is thread-safe and can be safely used by multiple threads.
    /// </summary>
    public class NexusClient : INexusClient
    {
        private readonly HttpClientWrapper _httpClient;
        private bool _disposed;
        
        /// <summary>
        /// Initializes a new instance of NexusClient
        /// </summary>
        /// <param name="options">Client configuration options</param>
        /// <param name="loggerFactory">Optional logger factory instance</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null</exception>
        /// <exception cref="SunbayBusinessException">Thrown when <see cref="NexusClientOptions.ApiKey"/> is null or whitespace</exception>
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
                    ApiConstants.ErrorCodeParameterError,
                    ApiConstants.MessageApiKeyRequired);
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
                ApiConstants.PathSale,
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
                ApiConstants.PathAuth,
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
                ApiConstants.PathForcedAuth,
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
                ApiConstants.PathIncrementalAuth,
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
                ApiConstants.PathPostAuth,
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
                ApiConstants.PathRefund,
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
                ApiConstants.PathVoid,
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
                ApiConstants.PathAbort,
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
                ApiConstants.PathTipAdjust,
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
                ApiConstants.PathQuery,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Batch close
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
                ApiConstants.PathBatchClose,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Batch query
        /// </summary>
        /// <param name="request">Batch query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Batch query response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<BatchQueryResponse> BatchQueryAsync(
            BatchQueryRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.GetAsync<BatchQueryRequest, BatchQueryResponse>(
                ApiConstants.PathBatchQuery,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create Hosted Payment Page checkout session. Redirect the customer to <see cref="CreateCheckoutSessionResponse.CheckoutUrl"/> to complete payment.
        /// See <see href="https://docs.sunbay.dev/en/refspec/online/checkout/checkout-api-integration">Create checkout session</see>.
        /// </summary>
        /// <param name="request">Create session request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Checkout URL and session metadata</returns>
        public async Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(
            CreateCheckoutSessionRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.PostAsync<CreateCheckoutSessionRequest, CreateCheckoutSessionResponse>(
                ApiConstants.PathCheckoutCreateSession,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Expire/close a checkout session.
        /// See <see href="https://docs.sunbay.dev/en/refspec/online/checkout/expire-session">Expire checkout session</see>.
        /// </summary>
        /// <param name="request">Expire session request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Expiration result</returns>
        public async Task<ExpireCheckoutSessionResponse> ExpireCheckoutSessionAsync(
            ExpireCheckoutSessionRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.PostAsync<ExpireCheckoutSessionRequest, ExpireCheckoutSessionResponse>(
                ApiConstants.PathCheckoutExpireSession,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Online direct payment without creating an HPP session first (e.g. Google Pay / Apple Pay with wallet token).
        /// See <see href="https://docs.sunbay.dev/en/refspec/online/direct-payment">Direct payment</see>.
        /// </summary>
        /// <param name="request">Direct payment request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Payment result</returns>
        public async Task<DirectPaymentResponse> DirectPaymentAsync(
            DirectPaymentRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.PostAsync<DirectPaymentRequest, DirectPaymentResponse>(
                ApiConstants.PathCheckoutSale,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Online refund for checkout transactions
        /// </summary>
        /// <param name="request">Online refund request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Online refund response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<OnlineRefundResponse> OnlineRefundAsync(
            OnlineRefundRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.PostAsync<OnlineRefundRequest, OnlineRefundResponse>(
                ApiConstants.PathCheckoutRefund,
                request,
                cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Query closed (settled) batch records. Supports filtering by payment channel and time range.
        /// If no time range is specified, the API returns data from the last 7 days by default.
        /// The maximum query span is 30 days.
        /// </summary>
        /// <param name="request">Batch close list request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Batch close list response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<BatchCloseListResponse> BatchCloseListAsync(
            BatchCloseListRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.GetAsync<BatchCloseListRequest, BatchCloseListResponse>(
                ApiConstants.PathBatchCloseList,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Query merchant information by SUNBAY platform merchant ID.
        /// </summary>
        /// <param name="request">Merchant query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Merchant query response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<MerchantQueryResponse> MerchantQueryAsync(
            MerchantQueryRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.GetAsync<MerchantQueryRequest, MerchantQueryResponse>(
                ApiConstants.PathMerchantQuery,
                request,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Query terminals bound to a merchant. Uses token-based pagination (up to 100 per page);
        /// pass <see cref="MerchantTerminalsQueryRequest.NextToken"/> from the previous response to fetch the next page.
        /// </summary>
        /// <param name="request">Merchant terminals query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Merchant terminals query response</returns>
        /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
        /// <exception cref="SunbayNetworkException">Thrown when network error occurs</exception>
        /// <exception cref="SunbayBusinessException">Thrown when business error occurs</exception>
        public async Task<MerchantTerminalsQueryResponse> MerchantTerminalsQueryAsync(
            MerchantTerminalsQueryRequest request,
            CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#else
            ArgumentNullException.ThrowIfNull(request);
#endif

            return await _httpClient.GetAsync<MerchantTerminalsQueryRequest, MerchantTerminalsQueryResponse>(
                ApiConstants.PathMerchantTerminalsQuery,
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
