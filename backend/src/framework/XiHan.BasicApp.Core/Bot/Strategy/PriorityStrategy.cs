// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Core.Bot.Consts;
using XiHan.BasicApp.Core.Bot.Core;
using XiHan.BasicApp.Core.Bot.Models;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.Strategy;

/// <summary>
/// 优先级策略
/// </summary>
public class PriorityStrategy : IBotStrategy
{
    private readonly ILogger<PriorityStrategy> _logger;

    /// <summary>
    /// 创建优先级策略
    /// </summary>
    public PriorityStrategy(ILogger<PriorityStrategy> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 策略名称
    /// </summary>
    public string Name => BotStrategyNames.Priority;

    /// <summary>
    /// 执行策略
    /// </summary>
    public async Task ExecuteAsync(BotContext context, IReadOnlyList<IBotProvider> providers)
    {
        var provider = providers.FirstOrDefault();
        if (provider is null)
        {
            return;
        }

        context.CancellationToken.ThrowIfCancellationRequested();
        var result = await SafeSendAsync(provider, context);
        context.AddResult(provider.Name, result);
    }

    private async Task<BotResult> SafeSendAsync(IBotProvider provider, BotContext context)
    {
        try
        {
            return await provider.SendAsync(context.Message, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bot provider exception. Provider: {Provider}", provider.Name);
            return BotResult.Failed(ex.Message, provider.Name);
        }
    }
}
