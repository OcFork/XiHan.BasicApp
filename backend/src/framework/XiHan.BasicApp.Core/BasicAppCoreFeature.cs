// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Application;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Caching.Abstractions;
using XiHan.Framework.Domain;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.ObjectStorage.Abstractions;
using XiHan.Framework.Observability.OpenTelemetry;
using XiHan.Framework.Persistence.Abstractions;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;
using XiHan.Framework.Scheduling.Abstractions;
using XiHan.Framework.Security.Abstractions;

namespace XiHan.BasicApp.Core;

/// <summary>
/// XiHanBasicAppCoreModule
/// </summary>
// xihan-migrate(V4-01): 已删除没有 5.0 能力的 4.x 模块：XiHanAIModule, XiHanBotModule, XiHanBotDingTalkModule, XiHanBotEmailModule, XiHanBotLarkModule, XiHanBotSmsModule, XiHanBotTelegramModule, XiHanBotWeComModule, XiHanCastleModule, XiHanHttpModule, XiHanLoggingModule, XiHanObjectMappingModule, XiHanScriptModule, XiHanSearchEnginesModule, XiHanSerializationModule, XiHanSettingsModule, XiHanTemplatingModule, XiHanThreadingModule, XiHanUpgradeModule, XiHanValidationModule, XiHanVirtualFileSystemModule, XiHanApplicationModule。
public sealed class BasicAppCoreFeature : IFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.core",
        Package = "XiHan.BasicApp.Core",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
            new CapabilityRequirement(SecurityCapabilities.Authentication, SecurityCapabilities.Version),
            new CapabilityRequirement(SecurityCapabilities.Authorization, SecurityCapabilities.Version),
            new CapabilityRequirement(SchedulingCapabilities.Jobs, SchedulingCapabilities.Version),
            new CapabilityRequirement(CachingCapabilities.Distributed, CachingCapabilities.Version),
            new CapabilityRequirement(PersistenceCapabilities.Relational, PersistenceCapabilities.Version),
            new CapabilityRequirement(MessagingCapabilities.Queue, MessagingCapabilities.Version),
            new CapabilityRequirement(LocalizationCapabilities.Resources, LocalizationCapabilities.Version),
            new CapabilityRequirement(MultiTenancyCapabilities.Context, MultiTenancyCapabilities.Version),
            new CapabilityRequirement(ObjectStorageCapabilities.Blob, ObjectStorageCapabilities.Version),
            new CapabilityRequirement(ObservabilityCapabilities.Export, ObservabilityCapabilities.Version),
        ],
        Entry = typeof(BasicAppCoreFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 架在 5.0 作用域 SqlSugar 客户端上的适配：4.x 的客户端解析器与显式工作单元管理器
        services.AddScoped<ISqlSugarClientResolver, ScopedSqlSugarClientResolver>();
        services.AddScoped<IUnitOfWorkManager, SqlSugarUnitOfWorkManager>();

        // 当前用户 / 租户：读 IInvocationAccessor.Current 的安全上下文（xihan migrate v4 生成）
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ICurrentTenant, CurrentTenant>();

        // 4.x 容器注册（xihan migrate v4 生成）
        services.AddCoreV4Services();

        // 应用信息（升级编排按实例标识区分节点）
        services.TryAddSingleton<IApplicationInfoAccessor, HostApplicationInfoAccessor>();

        // xihan-migrate(P5-04): 4.x 的 OIDC 签发（AddXiHanOidc：签名密钥提供器与 id_token 签发服务，XiHan:Authentication:Oidc:IsEnabled 门控）
        // 不在 5.0 里（5.0 只消费 OIDC），由 BasicApp 自行承接后在此注册。
    }
}
