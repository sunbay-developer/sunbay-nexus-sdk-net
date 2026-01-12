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
dotnet add package Sunbay.Nexus.Sdk --version 1.0.5
```

### PackageReference
```xml
<PackageReference Include="Sunbay.Nexus.Sdk" Version="1.0.5" />
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

The SDK provides the following transaction methods:

- `SaleAsync` - Execute a sale transaction
- `AuthAsync` - Authorization (pre-auth)
- `ForcedAuthAsync` - Forced authorization
- `IncrementalAuthAsync` - Incremental authorization
- `PostAuthAsync` - Post authorization
- `RefundAsync` - Refund transaction
- `VoidAsync` - Void transaction
- `AbortAsync` - Abort transaction
- `TipAdjustAsync` - Adjust tip amount
- `QueryAsync` - Query transaction status
- `BatchCloseAsync` - Batch close settlement
- `BatchQueryAsync` - Batch query settlement summary data

## Configuration Options

```csharp
var client = new NexusClient(new NexusClientOptions
{
    ApiKey = "sk_test_xxx",                      // Required
    BaseUrl = "https://open.sunbay.us",          // Optional, default: https://open.sunbay.us
    Timeout = TimeSpan.FromSeconds(30),          // Optional, default: 30 seconds
    MaxRetries = 3,                              // Optional, default: 3
    MaxTotalConnections = 200,                   // Optional, default: 200
    MaxConnectionsPerEndpoint = 20               // Optional, default: 20
});
```

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

In dependency injection scenarios, you can inject `ILoggerFactory` from the DI container:

```csharp
// In Startup.cs or Program.cs
services.AddSingleton<ILoggerFactory>(sp => 
    LoggerFactory.Create(builder => builder.AddConsole()));

// Then inject in your service
public class PaymentService
{
    private readonly NexusClient _client;
    
    public PaymentService(ILoggerFactory loggerFactory)
    {
        _client = new NexusClient(new NexusClientOptions
        {
            ApiKey = Environment.GetEnvironmentVariable("SUNBAY_API_KEY")!,
            BaseUrl = "https://open.sunbay.us"
        }, loggerFactory);
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

## License

MIT License. Copyright (c) 2025 Sunbay
