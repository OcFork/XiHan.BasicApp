// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Bot.Email.Abstractions;
using XiHan.BasicApp.Core.Bot.Email.Messaging;
using XiHan.BasicApp.Core.Bot.Email.Options;
using XiHan.BasicApp.Core.Bot.Email.Stores;
using XiHan.BasicApp.Core.Bot.Extensions;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.Email.Extensions;

/// <summary>
/// BotBuilder 邮件扩展
/// </summary>
public static class BotBuilderEmailExtensions
{
    /// <summary>
    /// 启用邮件提供者
    /// </summary>
    /// <param name="builder">Bot 构建器</param>
    /// <param name="configure">邮件配置委托</param>
    /// <returns>Bot 构建器</returns>
    public static BotBuilder UseEmail(this BotBuilder builder, Action<EmailOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.Configure(configure);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IBotProvider, EmailBotProvider>());
        builder.Services.TryAddSingleton<IEmailConfigStore, DefaultEmailConfigStore>();

        return builder;
    }
}
