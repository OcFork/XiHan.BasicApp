// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        var config = context.Configuration;

        // OIDC 签名密钥提供器与 id_token 签发服务（由 XiHan:Authentication:Oidc:IsEnabled 门控）
        _ = services.AddXiHanOidc(config);
    }
}
