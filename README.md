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
dotnet add package Sunbay.Nexus.Sdk --version 1.0.0
```

### PackageReference
```xml
<PackageReference Include="Sunbay.Nexus.Sdk" Version="1.0.0" />
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
        // Initialize logger factory
        using var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = loggerFactory.CreateLogger<Program>();
        
        // Get API key from environment variable or configuration
        // DO NOT hardcode sensitive information in source code
        var apiKey = Environment.GetEnvironmentVariable("SUNBAY_API_KEY") 
            ?? throw new InvalidOperationException("SUNBAY_API_KEY environment variable is required");
        
        // Initialize client
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
                    OrderAmount = 100.00m,
                    PricingCurrency = "USD"
                },
                Description = "Product purchase",
                TerminalSn = "T1234567890"
            };
            
            logger.LogInformation("Sending sale request - ReferenceOrderId: {ReferenceOrderId}, TransactionRequestId: {TransactionRequestId}", 
                request.ReferenceOrderId, request.TransactionRequestId);
            
            // Execute transaction
            var response = await client.SaleAsync(request);
            
            if (response.Success)
            {
                logger.LogInformation("Transaction successful - TransactionId: {TransactionId}", response.TransactionId);
            }
            else
            {
                logger.LogWarning("Transaction failed - Code: {Code}, Message: {Message}", response.Code, response.Message);
            }
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

Notes:
- The SDK only depends on `Microsoft.Extensions.Logging.Abstractions` (interfaces).
- You can plug in any logging provider (Console, Serilog, NLog, etc.) via `ILoggerFactory`.
- If you don't pass a logger factory, logging is disabled by default.

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
- System.Text.Json 8.0.0+
- Microsoft.Extensions.Http 8.0.0+

## Support

- Documentation: https://docs.sunbay.us
- Issues: https://github.com/sunbay-developer/sunbay-nexus-sdk-dotnet/issues

## License

MIT License. Copyright (c) 2025 Sunbay
