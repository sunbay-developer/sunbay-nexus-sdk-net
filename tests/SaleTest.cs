using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk;
using Sunbay.Nexus.Sdk.Models.Requests;
using Sunbay.Nexus.Sdk.Models.Common;
using Sunbay.Nexus.Sdk.Exceptions;

/// <summary>
/// Sale transaction test for Sunbay Nexus SDK
/// Note: This test requires a real API connection
/// </summary>
public class SaleTest
{
    public static async Task Main(string[] args)
    {
        // Initialize logger factory
        using var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = loggerFactory.CreateLogger<SaleTest>();
        
        // Initialize client with test credentials
        var client = new NexusClient(new NexusClientOptions
        {
            ApiKey = "mfgyn0hvs9teofvuad03jkwvmtrdm2sb",
            BaseUrl = "https://open.sunbay.dev",
            Timeout = TimeSpan.FromSeconds(60)
        }, loggerFactory);
        
        try
        {
            // Set timeExpire (ISO 8601 format: yyyy-MM-ddTHH:mm:sszzz)
            var expireTime = DateTimeOffset.UtcNow.AddMinutes(10);
            var timeExpire = expireTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            
            // Build amount
            var amount = new SaleAmount
            {
                OrderAmount = 222.00m,
                PricingCurrency = "USD"
            };
            
            // Build sale request
            var request = new SaleRequest
            {
                AppId = "test_sm6par3xf4d3tkum",
                MerchantId = "M1254947005",
                ReferenceOrderId = $"ORDER{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
                TransactionRequestId = $"PAY_REQ_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
                Amount = amount,
                Description = "Starbucks - Americano x2",
                TerminalSn = "TESTSN1764580772062",
                Attach = "{\"storeId\":\"STORE001\",\"tableNo\":\"T05\"}",
                NotifyUrl = "https://merchant.com/notify",
                TimeExpire = timeExpire
            };
            
            logger.LogInformation("Sending sale request - ReferenceOrderId: {ReferenceOrderId}, TransactionRequestId: {TransactionRequestId}", 
                request.ReferenceOrderId, request.TransactionRequestId);
            
            // Execute transaction
            var response = await client.SaleAsync(request);
            
            if (response.Success)
            {
                logger.LogInformation("Transaction successful - TransactionId: {TransactionId}, ReferenceOrderId: {ReferenceOrderId}, TransactionRequestId: {TransactionRequestId}", 
                    response.TransactionId, response.ReferenceOrderId, response.TransactionRequestId);
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
            logger.LogDebug("Client disposed");
        }
    }
}
