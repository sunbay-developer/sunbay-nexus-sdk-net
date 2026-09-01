using System;
using System.Threading;
using System.Threading.Tasks;
using Sunbay.Nexus.Sdk.Models.Requests;
using Sunbay.Nexus.Sdk.Models.Responses;

namespace Sunbay.Nexus.Sdk
{
    /// <summary>
    /// Contract for the Sunbay Nexus API client. Implementations are thread-safe and
    /// can be shared across concurrent callers. Register via DI or mock in tests.
    /// </summary>
    public interface INexusClient : IAsyncDisposable
    {
        /// <summary>Execute a sale transaction.</summary>
        Task<SaleResponse> SaleAsync(SaleRequest request, CancellationToken cancellationToken = default);

        /// <summary>Authorization (pre-auth).</summary>
        Task<AuthResponse> AuthAsync(AuthRequest request, CancellationToken cancellationToken = default);

        /// <summary>Forced authorization.</summary>
        Task<ForcedAuthResponse> ForcedAuthAsync(ForcedAuthRequest request, CancellationToken cancellationToken = default);

        /// <summary>Incremental authorization.</summary>
        Task<IncrementalAuthResponse> IncrementalAuthAsync(IncrementalAuthRequest request, CancellationToken cancellationToken = default);

        /// <summary>Post authorization (capture).</summary>
        Task<PostAuthResponse> PostAuthAsync(PostAuthRequest request, CancellationToken cancellationToken = default);

        /// <summary>Refund a transaction.</summary>
        Task<RefundResponse> RefundAsync(RefundRequest request, CancellationToken cancellationToken = default);

        /// <summary>Void a transaction.</summary>
        Task<VoidResponse> VoidAsync(VoidRequest request, CancellationToken cancellationToken = default);

        /// <summary>Abort an in-flight terminal transaction.</summary>
        Task<AbortResponse> AbortAsync(AbortRequest request, CancellationToken cancellationToken = default);

        /// <summary>Adjust the tip on a completed transaction.</summary>
        Task<TipAdjustResponse> TipAdjustAsync(TipAdjustRequest request, CancellationToken cancellationToken = default);

        /// <summary>Query a transaction by its identifiers.</summary>
        Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default);

        /// <summary>Trigger a batch close (settlement).</summary>
        Task<BatchCloseResponse> BatchCloseAsync(BatchCloseRequest request, CancellationToken cancellationToken = default);

        /// <summary>Query current-batch aggregated statistics.</summary>
        Task<BatchQueryResponse> BatchQueryAsync(BatchQueryRequest request, CancellationToken cancellationToken = default);

        /// <summary>Query closed (settled) batch records.</summary>
        Task<BatchCloseListResponse> BatchCloseListAsync(BatchCloseListRequest request, CancellationToken cancellationToken = default);

        /// <summary>Query merchant information.</summary>
        Task<MerchantQueryResponse> MerchantQueryAsync(MerchantQueryRequest request, CancellationToken cancellationToken = default);

        /// <summary>Query terminals bound to a merchant (token-based pagination).</summary>
        Task<MerchantTerminalsQueryResponse> MerchantTerminalsQueryAsync(MerchantTerminalsQueryRequest request, CancellationToken cancellationToken = default);

        /// <summary>Create Hosted Payment Page checkout session.</summary>
        Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(CreateCheckoutSessionRequest request, CancellationToken cancellationToken = default);

        /// <summary>Expire/close a checkout session.</summary>
        Task<ExpireCheckoutSessionResponse> ExpireCheckoutSessionAsync(ExpireCheckoutSessionRequest request, CancellationToken cancellationToken = default);

        /// <summary>Online direct payment without creating an HPP session first.</summary>
        Task<DirectPaymentResponse> DirectPaymentAsync(DirectPaymentRequest request, CancellationToken cancellationToken = default);

        /// <summary>Online refund for checkout transactions.</summary>
        Task<OnlineRefundResponse> OnlineRefundAsync(OnlineRefundRequest request, CancellationToken cancellationToken = default);
    }
}
