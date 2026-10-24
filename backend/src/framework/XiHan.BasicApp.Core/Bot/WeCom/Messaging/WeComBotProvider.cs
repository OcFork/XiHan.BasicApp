// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Bot.Consts;
using XiHan.BasicApp.Core.Bot.Core;
using XiHan.BasicApp.Core.Bot.Enums;
using XiHan.BasicApp.Core.Bot.Helpers;
using XiHan.BasicApp.Core.Bot.Models;
using XiHan.BasicApp.Core.Bot.Providers;
using XiHan.BasicApp.Core.Bot.WeCom.Abstractions;
using XiHan.BasicApp.Core.Bot.WeCom.Models;
using XiHan.BasicApp.Core.Bot.WeCom.Options;

namespace XiHan.BasicApp.Core.Bot.WeCom.Messaging;

/// <summary>
/// 企业微信 Bot 提供者
/// </summary>
public class WeComBotProvider : IBotProvider
{
    private readonly IWeComConfigStore _configStore;

    /// <summary>
    /// 创建提供者
    /// </summary>
    public WeComBotProvider(IWeComConfigStore configStore)
    {
        _configStore = configStore;
    }

    /// <summary>
    /// 提供者名称
    /// </summary>
    public string Name => BotProviderNames.WeCom;

    /// <summary>
    /// 发送消息
    /// </summary>
    public async Task<BotResult> SendAsync(BotMessage message, BotContext context)
    {
        var options = await _configStore.GetAsync(context.CancellationToken);
        if (options is null || !options.Enabled)
        {
            return BotResult.BadRequest("WeCom provider is not configured or disabled.", Name);
        }

        if (string.IsNullOrWhiteSpace(options.Key))
        {
            return BotResult.BadRequest("WeCom key is required.", Name);
        }

        var bot = new WeComBot(options);
        switch (message.Type)
        {
            case BotMessageType.Markdown:
                var markdown = new WeComMarkdown
                {
                    Content = message.Content
                };
                return BotResult.From(await bot.MarkdownMessage(markdown, context.CancellationToken), Name);

            case BotMessageType.Image:
                if (BotMessageHelper.TryGetData(message, WeComMessageDataKeys.WeComImage, out WeComImage? image) && image is not null)
                {
                    return BotResult.From(await bot.ImageMessage(image, context.CancellationToken), Name);
                }
                break;

            case BotMessageType.File:
                if (BotMessageHelper.TryGetData(message, WeComMessageDataKeys.WeComFile, out WeComFile? file) && file is not null)
                {
                    return BotResult.From(await bot.FileMessage(file, context.CancellationToken), Name);
                }
                break;

            case BotMessageType.Card:
                if (BotMessageHelper.TryGetData(message, WeComMessageDataKeys.WeComTemplateCardTextNotice, out WeComTemplateCardTextNotice? textNotice) && textNotice is not null)
                {
                    return BotResult.From(await bot.TextNoticeMessage(textNotice, context.CancellationToken), Name);
                }
                if (BotMessageHelper.TryGetData(message, WeComMessageDataKeys.WeComTemplateCardNewsNotice, out WeComTemplateCardNewsNotice? newsNotice) && newsNotice is not null)
                {
                    return BotResult.From(await bot.NewsNoticeMessage(newsNotice, context.CancellationToken), Name);
                }
                break;

            case BotMessageType.Link:
                if (BotMessageHelper.TryGetData(message, WeComMessageDataKeys.WeComNews, out WeComNews? news) && news is not null)
                {
                    return BotResult.From(await bot.NewsMessage(news, context.CancellationToken), Name);
                }
                break;
        }

        var text = new WeComText
        {
            Content = message.Content
        };
        ApplyMentions(text, message);
        return BotResult.From(await bot.TextMessage(text, context.CancellationToken), Name);
    }

    private static void ApplyMentions(WeComText text, BotMessage message)
    {
        if (message.Mentions.Count == 0)
        {
            return;
        }

        text.Mentions = message.Mentions;
        text.MentionedMobiles = message.Mentions;
    }
}
