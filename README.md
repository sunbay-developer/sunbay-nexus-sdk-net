# Sunbay Nexus SDK for .NET

Official .NET SDK for Sunbay Payment Platform

## Features

- ✅ Async/await support for high performance
- ✅ Multi-target framework support (.NET Standard 2.0, .NET 6.0, .NET 8.0)
- ✅ Automatic retry for transient failures
- ✅ Comprehensive exception handling
- ✅ Minimal dependencies
- ✅ Thread-safe client

## Installation

### Package Manager
```powershell
Install-Package Sunbay.Nexus.Sdk
```

### .NET CLI
```bash
dotnet add package Sunbay.Nexus.Sdk --version 1.0.16
```

### PackageReference
```xml
<PackageReference Include="Sunbay.Nexus.Sdk" Version="1.0.16" />
```

## Quick Start

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk;
using Sunbay.Nexus.Sdk.Models.Requests;
using Sunbay.Nexus.Sdk.Models.Common;
using Sunbay.Nexus.Sdk.Exceptions;

class Program
{
    static async Task Main(string[] args)
    {
        // Get API key from environment variable or configuration
        // DO NOT hardcode sensitive information in source code
        var apiKey = Environment.GetEnvironmentVariable("SUNBAY_API_KEY") 
            ?? throw new InvalidOperationException("SUNBAY_API_KEY environment variable is required");
        
        // Initialize logger factory (optional, but recommended for debugging)
        // Note: Passing ILoggerFactory is the mainstream C# SDK pattern (used by Azure SDK, AWS SDK, etc.)
        using var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = loggerFactory.CreateLogger<Program>();
        
        // Initialize client with logger factory
        var client = new NexusClient(new NexusClientOptions
        {
            ApiKey = apiKey,
            BaseUrl = "https://open.sunbay.us"
        }, loggerFactory);
        
        try
        {
            // Create sale request
            var request = new SaleRequest
            {
                AppId = "app_123456",
                MerchantId = "mch_789012",
                ReferenceOrderId = $"ORDER{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
                TransactionRequestId = Guid.NewGuid().ToString("N"),
                Amount = new SaleAmount
                {
                    OrderAmount = 10000L, // 100.00 USD in cents (smallest currency unit)
                    PriceCurrency = "USD"
                },
                Description = "Product purchase",
                TerminalSn = "T1234567890"
            };
            
            logger.LogInformation("Sending sale request - ReferenceOrderId: {ReferenceOrderId}, TransactionRequestId: {TransactionRequestId}", 
                request.ReferenceOrderId, request.TransactionRequestId);
            
            // Execute transaction
            // If code != "0", SunbayBusinessException will be thrown
            var response = await client.SaleAsync(request);
            
            logger.LogInformation("Transaction successful - TransactionId: {TransactionId}", response.TransactionId);
        }
        catch (SunbayNetworkException ex)
        {
            logger.LogError(ex, "Network error occurred - Message: {Message}, IsRetryable: {IsRetryable}", ex.Message, ex.IsRetryable);
        }
        catch (SunbayBusinessException ex)
        {
            logger.LogError("API error occurred - Code: {Code}, Message: {Message}, TraceId: {TraceId}", ex.Code, ex.Message, ex.TraceId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred - Message: {Message}", ex.Message);
        }
        finally
        {
            await client.DisposeAsync();
        }
    }
}
```

## Available API Methods

Semi-integration (terminal) transactions:
- `SaleAsync` - Execute a sale transaction
- `AuthAsync` - Authorization (pre-auth)
- `ForcedAuthAsync` - Forced authorization
- `IncrementalAuthAsync` - Incremental authorization
- `PostAuthAsync` - Post authorization (capture)
- `RefundAsync` - Refund transaction
- `VoidAsync` - Void transaction
- `AbortAsync` - Abort an in-flight terminal transaction
- `TipAdjustAsync` - Adjust tip amount

Query / settlement / merchant:
- `QueryAsync` - Query transaction status
- `BatchCloseAsync` - Trigger a batch close (settlement)
- `BatchQueryAsync` - Query current-batch aggregated statistics
- `BatchCloseListAsync` - Query closed (settled) batch records
- `MerchantQueryAsync` - Query merchant information
- `MerchantTerminalsQueryAsync` - Query terminals bound to a merchant (token-based pagination)

Online (Hosted Payment Page) checkout:
- `CreateCheckoutSessionAsync` - Create HPP checkout session
- `ExpireCheckoutSessionAsync` - Expire/close a checkout session
- `DirectPaymentAsync` - Direct payment without HPP session (e.g. Google Pay / Apple Pay)
- `OnlineRefundAsync` - Refund a checkout transaction

All methods are also exposed via the `INexusClient` interface, so you can inject or mock the client in tests.

## Request Parameter Updates

- `TipConfig` now supports `useHostConfig` (default `false`). When `true`, platform tip configuration is fully used and other `TipConfig` fields are ignored.
- `SaleRequest`, `AuthRequest`, and `RefundRequest` (refund without reference only) now support `signatureConfig`.
- `signatureEntryLocation` remains supported for compatibility, but is deprecated in favor of `signatureConfig`.
- `BatchCloseRequest` now supports `printReceipt` with values: `TOTAL`, `DETAIL`, `BOTH`, `NONE`, `AUTO`.

## Configuration Options

```csharp
var client = new NexusClient(new NexusClientOptions
{
    ApiKey = "sk_test_xxx",                                       // Required
    BaseUrl = "https://open.sunbay.us",                           // Optional, default: https://open.sunbay.us
    Timeout = TimeSpan.FromSeconds(30),                           // Optional, overall request timeout, default: 30s
    ConnectTimeout = TimeSpan.FromSeconds(10),                    // Optional, TCP/TLS connect timeout, default: 10s (net6+ only)
    MaxRetries = 3,                                               // Optional, GET-request retry attempts, default: 3
    MaxConnectionsPerEndpoint = 200,                              // Optional, per-endpoint pool cap, default: 200
    MaxTotalConnections = 200,                                    // Optional, kept for API compatibility (currently not enforced by .NET stack)
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),           // Optional, recycle to pick up DNS/LB changes, default: 5m (net6+ only)
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2)         // Optional, drop silently-dead idle connections, default: 2m (net6+ only)
});
```

### High-concurrency notes
- On .NET 6+ the SDK uses `SocketsHttpHandler` under the hood; `ConnectTimeout`, `PooledConnectionLifetime`, and `PooledConnectionIdleTimeout` are all honored.
- On .NET Standard 2.0 the SDK falls back to `HttpClientHandler`; only `Timeout` and `MaxConnectionsPerEndpoint` apply. The other pool knobs are ignored silently.
- `HttpClient` is created and cached inside the SDK client; **share one `NexusClient` instance across your process** and dispose it on shutdown.

## Logging

The SDK integrates with the standard .NET logging abstractions (`Microsoft.Extensions.Logging`).
Logging is **optional** and fully controlled by the application.

### Using ILoggerFactory (Recommended)

This is the **mainstream approach** in C# SDKs, allowing the SDK to create category-specific loggers internally.

```csharp
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk;

// Configure logger factory (example: console logging)
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole()
           .SetMinimumLevel(LogLevel.Information);
});

// Create client with logger factory
var client = new NexusClient(new NexusClientOptions
{
    ApiKey = "sk_test_xxx",
    BaseUrl = "https://open.sunbay.us"
}, loggerFactory);
```

### Using Dependency Injection (ASP.NET Core)

In dependency injection scenarios, register `INexusClient` so consumers can depend on the interface (and mock it in tests):

```csharp
// In Program.cs
services.AddSingleton<INexusClient>(sp =>
    new NexusClient(new NexusClientOptions
    {
        ApiKey = Environment.GetEnvironmentVariable("SUNBAY_API_KEY")!,
        BaseUrl = "https://open.sunbay.us"
    }, sp.GetService<ILoggerFactory>()));

// Then inject in your service
public class PaymentService
{
    private readonly INexusClient _client;

    public PaymentService(INexusClient client)
    {
        _client = client;
    }
}
```

### Without Logging

If you don't pass a logger factory, logging is disabled by default:

```csharp
var client = new NexusClient(new NexusClientOptions
{
    ApiKey = "sk_test_xxx",
    BaseUrl = "https://open.sunbay.us"
});
// No logging will be performed
```

Notes:
- The SDK only depends on `Microsoft.Extensions.Logging.Abstractions` (interfaces).
- You can plug in any logging provider (Console, Serilog, NLog, Application Insights, etc.) via `ILoggerFactory`.
- The SDK creates category-specific loggers internally (e.g., `"Sunbay.Nexus.Sdk.Http.HttpClientWrapper"`).
- This approach follows the **mainstream C# SDK pattern** used by Azure SDK, AWS SDK, and other major .NET libraries.

## Exception Handling

The SDK throws two types of exceptions:

### SunbayNetworkException
Network-related errors (connection timeout, network error, etc.)
- `IsRetryable`: Indicates if the operation can be retried

### SunbayBusinessException
Business logic errors (parameter validation, API business errors, etc.)
- `Code`: Error code
- `Message`: Error message
- `TraceId`: Trace ID for debugging

## Requirements

- .NET Standard 2.0+ / .NET 6.0+ / .NET 8.0+
- System.Text.Json 8.0.0+ (for .NET Standard 2.0)
- Microsoft.Extensions.Http 8.0.0+
- (The SDK itself references `Microsoft.Extensions.Logging.Abstractions`, but this is a transitive dependency of the NuGet package; you don't need to install it manually.)

## Support

- Documentation: https://docs.sunbay.us
- Issues: https://github.com/sunbay-developer/sunbay-nexus-sdk-dotnet/issues

## Publish to NuGet

1. Update version in `src/Sunbay.Nexus.Sdk/Sunbay.Nexus.Sdk.csproj` (`<Version>`).
2. Run:

```bash
python3 deploy.py
```

`deploy.py` will:
- read `PackageId` and `Version` from the `.csproj`
- pack Release `.nupkg`
- prompt for NuGet API Key (hidden input)
- push package to `https://api.nuget.org/v3/index.json`

## License

MIT License. Copyright (c) 2025 Sunbay
