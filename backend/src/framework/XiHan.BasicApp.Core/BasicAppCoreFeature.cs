// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Scriban;
using XiHan.BasicApp.Core.Application;
using XiHan.BasicApp.Core.Auditing.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Authorization.Conditions;
using XiHan.BasicApp.Core.Bot.DingTalk.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Email.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Lark.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Sms.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Telegram.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Bot.Telegram.Options;
using XiHan.BasicApp.Core.Bot.WeCom.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Caching;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Localization;
using XiHan.BasicApp.Core.Messaging;
using XiHan.BasicApp.Core.ObjectStorage.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Tasks.ScheduledJobs.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Templating.Engines;
using XiHan.BasicApp.Core.Templating.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Timing.Extensions.DependencyInjection;
using XiHan.Framework.Caching.Abstractions;
using XiHan.Framework.Domain;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Kernel;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.ObjectStorage.Abstractions;
using XiHan.Framework.Observability.OpenTelemetry;
using XiHan.Framework.Persistence.Abstractions;
using XiHan.Framework.Primitives.Manifests;
using XiHan.Framework.Primitives;
using XiHan.Framework.Scheduling.Abstractions;
using XiHan.Framework.Security.Abstractions;
using XiHan.BasicApp.Core.Upgrade.Extensions;
using XiHan.BasicApp.Core.Data.SqlSugar.Initializers;
using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Core.Exceptions;

namespace XiHan.BasicApp.Core;

/// <summary>
/// XiHanBasicAppCoreModule
/// </summary>
// xihan-migrate(V4-01): 已删除没有 5.0 能力的 4.x 模块：XiHanAIModule, XiHanBotModule, XiHanBotDingTalkModule, XiHanBotEmailModule, XiHanBotLarkModule, XiHanBotSmsModule, XiHanBotTelegramModule, XiHanBotWeComModule, XiHanCastleModule, XiHanHttpModule, XiHanLoggingModule, XiHanObjectMappingModule, XiHanScriptModule, XiHanSearchEnginesModule, XiHanSerializationModule, XiHanSettingsModule, XiHanTemplatingModule, XiHanThreadingModule, XiHanUpgradeModule, XiHanValidationModule, XiHanVirtualFileSystemModule, XiHanApplicationModule。
public sealed class BasicAppCoreFeature : IHostedFeature
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

        // 4.x 数据层（多连接 SqlSugarScope、租户与模块库路由、仓储、初始化器）与显式工作单元管理器；作用域 ISqlSugarClient 由解析器给出，5.0 持久化特性以它装配
        // 分布式 ID（雪花 / 顺序 GUID，配置节 XiHan:DistributedIds）：数据层的审计处理器启动时就要它
        services.AddXiHanDistributedIds(context.Configuration);
        // 密码哈希与密码策略选项（原 4.x Security 模块）
        services.AddXiHanSecurityServices(context.Configuration);
        services.AddBasicAppData(context.Configuration);
        services.AddScoped<IUnitOfWorkManager, SqlSugarUnitOfWorkManager>();
        // 单例服务（发件箱、访问策略快照）只读当前工作单元，用单例访问器而不是作用域的管理器
        services.TryAddSingleton<ICurrentUnitOfWorkAccessor, CurrentUnitOfWorkAccessor>();

        // 当前用户 / 租户：读 IInvocationAccessor.Current 的安全上下文（xihan migrate v4 生成）
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ICurrentTenant, CurrentTenant>();
        // 4.x 形态的主体访问器：读调用主体，Change 给后台任务切换执行身份
        services.AddScoped<ICurrentPrincipalAccessor, CurrentPrincipalAccessor>();
        services.TryAddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
        // 调用携带的租户登记进执行流环境（SqlSugar 全局租户过滤器按查询即时读它），挂在租户解析之后
        context.AddMiddleware<TenantAmbientMiddleware>(PipelineStage.Tenant);
        // 业务异常翻译成调用结果（400/404/409/403），挂在 Observe 末尾、内核 ObserveMiddleware 之内；未识别的异常仍由内核记 500
        context.AddMiddleware<AppExceptionMiddleware>(PipelineStage.Observe);

        // 4.x 容器注册（xihan migrate v4 生成）
        services.AddCoreV4Services();

        // 承接进 Core 的 4.x 模块（原各自模块的 ConfigureServices）
        var configuration = context.Configuration;
        services.AddBasicAppCaching(configuration);
        services.AddXiHanTiming();
        services.AddXiHanTemplating();
        _ = services.AddXiHanTasks(configuration);
        services.AddXiHanBot();
        services.AddXiHanBotEmail();
        services.AddXiHanBotSms();
        services.AddXiHanBotTelegram();
        services.Configure<TelegramBotPlatformOptions>(configuration.GetSection(TelegramBotPlatformOptions.SectionName));
        services.AddXiHanBotTelegramPlatform();
        services.AddXiHanBotDingTalk();
        services.AddXiHanBotLark();
        services.AddXiHanBotWeCom();
        services.AddXiHanObjectStorage(configuration);
        services.AddXiHanAuditing(configuration);
        // 升级（版本库、迁移执行器、维护模式；Web.Core 再把维护模式管理器换成应用实现）
        services.AddXiHanUpgrade(configuration);
        services.AddXiHanMessaging();
        services.AddBasicAppConditions();
        services.Configure<XiHanLocalizationOptions>(configuration.GetSection(XiHanLocalizationOptions.SectionName));
        services.TryAddSingleton<IEnumLocalizationService, EnumLocalizationService>();

        // 应用信息（升级编排按实例标识区分节点）
        services.TryAddSingleton<IApplicationInfoAccessor, HostApplicationInfoAccessor>();

    }

    /// <inheritdoc />
    public async Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        // 数据库初始化（原 4.x Data 模块的 OnApplicationInitialization）：建库 → 建表 → 种子，各级由 XiHan:Data:SqlSugarCore 的开关控制；
        // 必须先于其他特性的启动钩子（它们会查表），失败就让宿主启动失败而不是带着残缺的库跑
        var logger = services.GetRequiredService<ILogger<BasicAppCoreFeature>>();
        await using (var scope = services.CreateAsyncScope())
        {
            logger.LogInformation("Initializing databases (create / tables / seed) ...");
            await scope.ServiceProvider.GetRequiredService<IDbInitializer>().InitializeAsync().ConfigureAwait(false);
            logger.LogInformation("Database initialization finished.");
        }

        // 模板引擎登记（原 4.x Templating 模块的 OnApplicationInitialization）
        var registry = services.GetRequiredService<ITemplateEngineRegistry>();
        registry.RegisterEngine("Scriban", services.GetRequiredService<ITemplateEngine<Template>>());
        registry.RegisterEngine("String", services.GetRequiredService<ITemplateEngine<string>>());
        registry.SetDefaultEngine<Template>("Scriban");
        registry.SetDefaultEngine<string>("String");
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
