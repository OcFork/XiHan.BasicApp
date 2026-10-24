// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Bot.Core;
using XiHan.BasicApp.Core.Bot.Models;

namespace XiHan.BasicApp.Core.Bot.Providers;

/// <summary>
/// Bot 提供者抽象
/// </summary>
public interface IBotProvider
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 由提供者发送消息
    /// </summary>
    Task<BotResult> SendAsync(BotMessage message, BotContext context);
}
