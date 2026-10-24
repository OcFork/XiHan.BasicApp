// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Bot.Extensions;
using XiHan.BasicApp.Core.Bot.Lark.Abstractions;
using XiHan.BasicApp.Core.Bot.Lark.Messaging;
using XiHan.BasicApp.Core.Bot.Lark.Options;
using XiHan.BasicApp.Core.Bot.Lark.Stores;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.Lark.Extensions;

/// <summary>
/// BotBuilder 飞书扩展
/// </summary>
public static class BotBuilderLarkExtensions
{
    /// <summary>
    /// 启用飞书提供者
    /// </summary>
    /// <param name="builder">Bot 构建器</param>
    /// <param name="configure">飞书配置委托</param>
    /// <returns>Bot 构建器</returns>
    public static BotBuilder UseLark(this BotBuilder builder, Action<LarkOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.Configure(configure);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IBotProvider, LarkBotProvider>());
        builder.Services.TryAddSingleton<ILarkConfigStore, DefaultLarkConfigStore>();

        return builder;
    }
}
