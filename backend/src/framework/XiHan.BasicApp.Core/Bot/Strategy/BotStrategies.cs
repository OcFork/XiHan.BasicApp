// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiHan.BasicApp.Core.Bot.Consts;
using XiHan.BasicApp.Core.Bot.Core;
using XiHan.BasicApp.Core.Bot.Models;
using XiHan.BasicApp.Core.Bot.Options;
using XiHan.BasicApp.Core.Bot.Providers;

namespace XiHan.BasicApp.Core.Bot.Strategy;

/// <summary>
/// 广播策略
/// </summary>
public class BroadcastStrategy : IBotStrategy
{
    private readonly XiHanBotOptions _options;
    private readonly ILogger<BroadcastStrategy> _logger;

    /// <summary>
    /// 创建广播策略
    /// </summary>
    public BroadcastStrategy(IOptions<XiHanBotOptions> options, ILogger<BroadcastStrategy> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// 策略名称
    /// </summary>
    public string Name => BotStrategyNames.Broadcast;

    /// <summary>
    /// 执行策略
    /// </summary>
    public async Task ExecuteAsync(BotContext context, IReadOnlyList<IBotProvider> providers)
    {
        foreach (var provider in providers)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            var result = await SafeSendAsync(provider, context);
            context.AddResult(provider.Name, result);

            if (!result.IsSuccess && !_options.ContinueOnError)
            {
                break;
            }
        }
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
