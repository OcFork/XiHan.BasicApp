// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Bot.DingTalk.Abstractions;
using XiHan.BasicApp.Core.Bot.DingTalk.Messaging;
using XiHan.BasicApp.Core.Bot.DingTalk.Options;
using XiHan.BasicApp.Core.Bot.DingTalk.Stores;
using XiHan.BasicApp.Core.Bot.Extensions;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.DingTalk.Extensions;

/// <summary>
/// BotBuilder 钉钉扩展
/// </summary>
public static class BotBuilderDingTalkExtensions
{
    /// <summary>
    /// 启用钉钉提供者
    /// </summary>
    /// <param name="builder">Bot 构建器</param>
    /// <param name="configure">钉钉配置委托</param>
    /// <returns>Bot 构建器</returns>
    public static BotBuilder UseDingTalk(this BotBuilder builder, Action<DingTalkOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.Configure(configure);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IBotProvider, DingTalkBotProvider>());
        builder.Services.TryAddSingleton<IDingTalkConfigStore, DefaultDingTalkConfigStore>();

        return builder;
    }
}
