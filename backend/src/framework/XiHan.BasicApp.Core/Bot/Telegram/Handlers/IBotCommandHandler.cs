// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Bot.Telegram.Core;

namespace XiHan.BasicApp.Core.Bot.Telegram.Handlers;

/// <summary>
/// Telegram 命令处理器（如 /start、/help；须配合 <see cref="BotCommandAttribute"/> 使用）
/// </summary>
public interface IBotCommandHandler
{
    /// <summary>
    /// 处理命令请求
    /// </summary>
    /// <param name="context">更新上下文</param>
    /// <param name="args">命令参数（正则直达时为捕获组）</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task HandleAsync(TelegramBotContext context, string[] args, CancellationToken cancellationToken = default);
}
