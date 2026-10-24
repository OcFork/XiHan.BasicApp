// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.WebHost.HealthChecks;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;

namespace XiHan.BasicApp.WebHost;

/// <summary>
/// 宿主特性：ASP.NET 健康检查项（端点与中间件在 Program.cs 里挂到 app.UseXiHan() 之后）
/// </summary>
public sealed class BasicAppWebHostFeature : IFeature
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
        // 第一批不含 AI 模块，向量库检查随 AI 模块（5.1）回归
        context.Services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database")
            .AddCheck<RedisHealthCheck>("redis");
    }
}
