# Sunbay Nexus SDK for .NET - 设计文档（精简版）

> 本文档已按当前代码实现整理，删除了未实现/未来规划的内容，只保留**现在 SDK 真正具备的能力**，方便后续维护和对外共享。

## 1. 项目概述

### 1.1 目标
- 为 Sunbay 支付平台提供一套 **简单、专业、易集成** 的 .NET SDK。
- 主要覆盖 **半集成支付场景**，当前已经实现 **Sale 交易**。

### 1.2 设计原则
- **一致性**：与 Java SDK 的业务语义、错误模型、URL 设计保持一致。
- **兼容性**：一套代码，多目标编译：
  - `netstandard2.0`
  - `net6.0`
  - `net8.0`
- **安全性**：不硬编码敏感信息，推荐使用环境变量或配置。
- **可观测性**：通过 `ILoggerFactory` 支持标准 .NET 日志体系。

## 2. 技术选型与关键决策

### 2.1 语言与运行时
- 语言：**C#**
- 目标框架（与代码保持一致）：

```xml
<TargetFrameworks>netstandard2.0;net6.0;net8.0</TargetFrameworks>
```

### 2.2 HTTP 与 JSON
- HTTP 客户端：`System.Net.Http.HttpClient`
  - SDK 自行管理 `HttpClientHandler`，避免频繁创建导致 Socket 耗尽。
  - 通过 `NexusClientOptions` 配置：
    - `Timeout`
    - 最大重试次数等。
- JSON 序列化：`System.Text.Json`
  - 在 `netstandard2.0` 下通过 NuGet 引入 `System.Text.Json`。

### 2.3 日志
- SDK 仅依赖 `Microsoft.Extensions.Logging.Abstractions`（接口）。
- 对外暴露 `ILoggerFactory?`：
  - 由调用方决定使用 Console / Serilog / NLog / Application Insights 等。
  - 未提供则不输出日志。

## 3. 项目结构（按当前代码）

```text
sunbay-nexus-sdk-dotnet/
├── src/
│   └── Sunbay.Nexus.Sdk/
│       ├── NexusClient.cs              # 主客户端
│       ├── NexusClientOptions.cs       # 配置项
│       ├── Http/
│       │   └── HttpClientWrapper.cs    # HTTP 封装与重试
│       ├── Exceptions/
│       │   ├── SunbayException.cs
│       │   ├── SunbayNetworkException.cs
│       │   └── SunbayBusinessException.cs
│       ├── Constants/
│       │   └── ApiConstants.cs         # URL 前缀、错误码、提示文案
│       └── Utilities/
│           └── IdGenerator.cs          # 请求 ID 生成
├── tests/
│   ├── SaleTest.cs                     # 控制台测试程序（调用 Sale）
│   └── SaleTest.csproj
└── README.md                           # 使用说明
```

> 说明：当前没有 `INexusClient` 接口、单元测试工程、CI 配置等，设计文档不再描述这些“未来规划”内容。

## 4. 核心 API 设计（按当前实现）

### 4.1 NexusClient

```csharp
public sealed class NexusClient : IAsyncDisposable
{
    public NexusClient(NexusClientOptions options, ILoggerFactory? loggerFactory = null);

    public Task<SaleResponse> SaleAsync(
        SaleRequest request, 
        CancellationToken cancellationToken = default);
    
    public ValueTask DisposeAsync();
}
```

特性：
- 所有外部调用（当前只有 `SaleAsync`）均为 **异步** 方法。
- `DisposeAsync` 负责释放内部 `HttpClient` 等资源：
  - 在 `netstandard2.0` 下通过条件编译处理 `IAsyncDisposable` 差异。

### 4.2 NexusClientOptions（与代码一致）

```csharp
public sealed class NexusClientOptions
{
    public string ApiKey { get; set; }                  // 必填
    public string BaseUrl { get; set; }                 // 默认：https://open.sunbay.us
    public TimeSpan Timeout { get; set; }               // 默认：30 秒
    public int MaxRetries { get; set; }                 // 默认：3（仅针对网络可重试错误）
    public int MaxTotalConnections { get; set; }        // 默认：200
    public int MaxConnectionsPerEndpoint { get; set; }  // 默认：20
}
```

### 4.3 异常模型（与实现对齐）

- `SunbayException`：SDK 内部自定义异常基类。
- `SunbayNetworkException`：
  - 用于网络层错误（超时、连接失败、5xx 等）。
  - 重要属性：
    - `Code` 固定为 `ApiConstants.ERROR_CODE_NETWORK_ERROR`
    - `IsRetryable` 标识是否适合重试。
- `SunbayBusinessException`：
  - 用于业务错误（Cxx、Bxx 等业务码）。
  - 包含：
    - `Code`（例如 `"C17"`）
    - `Message`
    - `TraceId`（便于排查问题）。

## 5. HTTP 通信与重试设计

### 5.1 HttpClientWrapper 职责
- 统一封装所有 HTTP 调用逻辑：
  - 构建 URL（含 query 参数反射拼接）。
  - 统一设置 Header：
    - `Authorization` / `X-Client-Request-Id` / `X-Timestamp` 等。
  - 统一序列化请求、反序列化响应。
  - 统一异常转换为 `SunbayNetworkException` / `SunbayBusinessException`。
- 对外只暴露 **泛型方法**：

```csharp
Task<TResponse> PostAsync<TRequest, TResponse>(...);
Task<TResponse> GetAsync<TResponse>(...);
```

### 5.2 响应解析（与 Java 行为对齐）
- 网关返回 JSON 结构：

```json
{
  "code": "0",
  "msg": "Success",
  "data": {
    "transactionId": "...",
    "referenceOrderId": "...",
    "transactionRequestId": "..."
  },
  "traceId": "..."
}
```

- 解析策略：
  - 先反序列化为基础响应模型（含 `code` / `msg` / `traceId`）。
  - 如果有 `data` 字段，再反序列化并合并到具体 `TResponse`（与 Java `parseResponse` 逻辑保持一致）。

### 5.3 重试策略（当前实现）
- 仅对 **网络类错误** 触发重试（`SunbayNetworkException.IsRetryable == true`）。
- 最大重试次数：`NexusClientOptions.MaxRetries`，默认 3。
- 每次重试前记录日志：

```csharp
_logger?.LogWarning(
    "Request failed, retrying ({Retry}/{MaxRetries}) ...",
    retryCount, _options.MaxRetries);
```

> 说明：目前主要针对 GET/网络错误的通用逻辑，Sale 为 POST 请求，不会做“幂等重试”以避免重复扣款风险，行为与 Java SDK 保持一致的保守策略。

## 6. 日志设计

### 6.1 对外约定
- SDK 不绑定任何具体日志实现，只依赖：

```xml
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />
```

- 通过构造函数注入 `ILoggerFactory?`：
  - 不传：不输出日志。
  - 传入：内部创建分类 Logger，例如：
    - `"Sunbay.Nexus.Sdk.NexusClient"`
    - `"Sunbay.Nexus.Sdk.Http.HttpClientWrapper"`

### 6.2 使用示例（与 README 一致）

```csharp
using Microsoft.Extensions.Logging;
using Sunbay.Nexus.Sdk;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole()
           .SetMinimumLevel(LogLevel.Information);
});

        var client = new NexusClient(new NexusClientOptions
        {
    ApiKey = Environment.GetEnvironmentVariable("SUNBAY_API_KEY")!,
            BaseUrl = "https://open.sunbay.us"
}, loggerFactory);
```

## 7. 模型与常量设计

### 7.1 模型
- 请求/响应模型全部使用 **class + 属性 get/set**，兼容 `netstandard2.0`：
  - `SaleRequest`, `SaleAmount`, `SaleResponse` 等。
- 不使用 `record` / `init` 等 C# 9 语法，避免低版本编译器不兼容。

### 7.2 常量
- 所有 URL 前缀、路径、错误码、通用错误文案集中在 `ApiConstants`：
  - `SEMI_INTEGRATION_PREFIX`
  - `COMMON_PREFIX`
  - 各种 `PATH_*`
  - 错误码：`ERROR_CODE_NETWORK_ERROR`, `ERROR_CODE_INVALID_RESPONSE` 等。
  - 提示文案：`MESSAGE_API_KEY_REQUIRED`, `MESSAGE_FAILED_PARSE_RESPONSE` 等。

## 8. 测试与示例

### 8.1 SaleTest 控制台示例
- 位置：`tests/SaleTest.cs`
- 作用：
  - 验证当前 SDK 与 Sunbay 测试环境的联通性。
  - 展示推荐用法（环境变量配置、ILoggerFactory、结构化日志）。
- 典型流程：
  1. 创建 `LoggerFactory`，启用 `AddConsole()`。
  2. 使用测试凭证创建 `NexusClient`。
  3. 构造 `SaleRequest`（含 `timeExpire`、`attach` 等字段）。
  4. 调用 `SaleAsync` 并打印结果。

## 9. NuGet 与版本策略（按当前 csproj）

### 9.1 打包配置（简要）
- `Sunbay.Nexus.Sdk.csproj` 中已配置：
  - `PackageId = Sunbay.Nexus.Sdk`
  - `Version = 1.0.0`
  - `PackageLicenseExpression = MIT`
  - `PackageReadmeFile = README.md`
- 依赖：
  - `System.Text.Json`（仅 `netstandard2.0`）
  - `Microsoft.Extensions.Logging.Abstractions`

### 9.2 版本策略
- 使用语义化版本（SemVer）：
  - `1.0.0`：当前这一版的最小可用版本（支持 Sale）。
  - 后续若扩展更多交易接口（Auth/Refund 等），按照 `1.x` 逐步演进。

## 10. 元信息

- **文档版本**：1.1  
- **最后更新**：2025-12-22  
- **覆盖范围**：仅描述当前代码仓库已实现的功能与设计，不再包含未落地的规划项。  
