// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Bot.Extensions;
using XiHan.BasicApp.Core.Bot.Providers;
using XiHan.BasicApp.Core.Bot.Telegram.Abstractions;
using XiHan.BasicApp.Core.Bot.Telegram.Messaging;
using XiHan.BasicApp.Core.Bot.Telegram.Options;
using XiHan.BasicApp.Core.Bot.Telegram.Stores;

namespace XiHan.BasicApp.Core.Bot.Telegram.Extensions;

/// <summary>
/// BotBuilder Telegram 扩展
/// </summary>
public static class BotBuilderTelegramExtensions
{
    /// <summary>
    /// 启用 Telegram 提供者
    /// </summary>
    /// <param name="builder">Bot 构建器</param>
    /// <param name="configure">Telegram 配置委托</param>
    /// <returns>Bot 构建器</returns>
    public static BotBuilder UseTelegram(this BotBuilder builder, Action<TelegramOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.Configure(configure);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IBotProvider, TelegramBotProvider>());
        builder.Services.TryAddSingleton<ITelegramConfigStore, DefaultTelegramConfigStore>();

        return builder;
    }
}
