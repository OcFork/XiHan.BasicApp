// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.RegularExpressions;

namespace XiHan.BasicApp.Core.Events;

/// <summary>
/// 领域事件类型 → 消息主题：与 <c>xihan migrate v4</c> 给处理器生成的主题常量同一规则（类型名拆词、去掉尾部的 Event / Domain、用 <c>-</c> 连接）
/// </summary>
public static partial class DomainEventTopics
{
    /// <summary>
    /// 事件类型的主题
    /// </summary>
    /// <param name="eventType">事件类型</param>
    public static string For(Type eventType)
    {
        ArgumentNullException.ThrowIfNull(eventType);
        var name = eventType.Name;
        var tick = name.IndexOf('`', StringComparison.Ordinal);
        if (tick > 0)
        {
            name = name[..tick];
        }

        var words = WordPattern().Matches(name).Select(static match => match.Value.ToLowerInvariant()).ToList();
        if (words.Count > 1 && words[^1] == "event")
        {
            words.RemoveAt(words.Count - 1);
        }

        if (words.Count > 1 && words[^1] == "domain")
        {
            words.RemoveAt(words.Count - 1);
        }

        return words.Count == 0 ? "event" : string.Join('-', words);
    }

    /// <summary>
    /// 单词：连续大写（后面不是小写）或可选大写开头的小写与数字串
    /// </summary>
    [GeneratedRegex("[A-Z]+(?![a-z])|[A-Z]?[a-z0-9]+")]
    private static partial Regex WordPattern();
}
