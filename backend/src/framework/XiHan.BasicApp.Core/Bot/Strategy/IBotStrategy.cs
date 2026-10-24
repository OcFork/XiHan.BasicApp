// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Bot.Core;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.Strategy;

/// <summary>
/// Bot 策略抽象
/// </summary>
public interface IBotStrategy
{
    /// <summary>
    /// 策略名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 使用提供者执行策略
    /// </summary>
    Task ExecuteAsync(BotContext context, IReadOnlyList<IBotProvider> providers);
}
