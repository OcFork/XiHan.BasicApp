// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using XiHan.BasicApp.Chat.Extensions;
using XiHan.BasicApp.Chat.Hubs;
using XiHan.BasicApp.Saas;
using XiHan.BasicApp.Web.Core.RealTime.Constants;
using XiHan.BasicApp.Web.Core.RealTime.Extensions;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Kernel;
using XiHan.Framework.Primitives.Manifests;
using XiHan.Framework.Primitives;
using XiHan.BasicApp.Chat.Infrastructure.Seeders;
using XiHan.BasicApp.Saas.Infrastructure.Security;

namespace XiHan.BasicApp.Chat;

/// <summary>
/// 曦寒基础应用在线聊天模块
/// </summary>
/// <remarks>
/// 独立一等模块（与 AI/代码生成/工作流/打印同构）：单聊/群聊/部门群/AI 助手会话、
/// 消息与表情回应、SignalR 实时推送、敏感词拦截、保留期清理与管理侧合规审计。
/// 依赖 Saas（复用 RBAC 表、SaasRepository、组织事件、菜单/权限种子基类）。
/// </remarks>
public sealed class BasicAppChatFeature : IHostedFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.chat",
        Package = "XiHan.BasicApp.Chat",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
        ],
        Entry = typeof(BasicAppChatFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 登记本程序集全部 [Operation]（源生成器产出）与本模块权限目录
        context.AddGeneratedOperations();
        context.AddPermissionSeeds(ChatPermissionCatalogSeeder.Seeds);

        // 种子（权限 → 菜单 → 角色授权 → 任务 → 配置）+ 领域服务 + 事件处理器
        services.AddChatDataSeeders();
        services.AddChatDomainServices();
        services.AddChatEventHandlers();
    }

    /// <inheritdoc />
    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        // xihan-migrate(P5-04): 端点改到宿主 Program.cs 的 app.UseXiHan() 之后注册：
        //   MapXiHanHub<BasicAppChatHub>(SignalRConstants.HubPaths.Chat))

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
