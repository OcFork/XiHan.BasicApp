// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using XiHan.BasicApp.AI;
using XiHan.BasicApp.Chat;
using XiHan.BasicApp.CodeGeneration;
using XiHan.BasicApp.Printing;
using XiHan.BasicApp.Saas;
using XiHan.BasicApp.Workflow;
using XiHan.BasicApp.WebHost.HealthChecks;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;

namespace XiHan.BasicApp.WebHost;

/// <summary>
/// 曦寒基础应用 Web 主机
/// </summary>
public sealed class BasicAppWebHostFeature : IHostedFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.webhost",
        Package = "XiHan.BasicApp.WebHost",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
        ],
        Entry = typeof(BasicAppWebHostFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        context.Services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database")
            .AddCheck<RedisHealthCheck>("redis")
            .AddCheck<QdrantHealthCheck>("qdrant");
    }

    /// <inheritdoc />
    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        // xihan-migrate(V4-01): GetApplicationBuilder() 在 StartAsync 里没有对应物；中间件与端点改到 Program.cs 的 app.UseXiHan() 之后注册。
        var app = context.GetApplicationBuilder();
        _ = app.UseTelegramBotWebhook();

        // xihan-migrate(V4-01): GetApplicationBuilder() 在 StartAsync 里没有对应物；中间件与端点改到 Program.cs 的 app.UseXiHan() 之后注册。
        var app = context.GetApplicationBuilder();

        var options = new HealthCheckOptions
        {
            ResponseWriter = WriteMinimalHealthResponseAsync
        };

        if (app is IEndpointRouteBuilder endpoints)
        {
            _ = endpoints.MapHealthChecks("/health", options).AllowAnonymous();
        }
        else
        {
            // 兜底：非端点路由构建器时退化为中间件（可能受 FallbackPolicy 影响）
            _ = app.UseHealthChecks("/health", options);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 最小化健康响应：仅总状态 + 各检查项名称与状态，不含连接串/异常/描述等敏感细节
    /// </summary>
    private static Task WriteMinimalHealthResponseAsync(HttpContext httpContext, HealthReport report)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";
        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString()
            })
        };
        return httpContext.Response.WriteAsJsonAsync(payload);
    }
}
