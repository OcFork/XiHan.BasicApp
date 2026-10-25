// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Caching.Abstractions.Stores;
using XiHan.Framework.Caching.Abstractions;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 应用分布式缓存选项
/// </summary>
public sealed class DistributedCacheOptions
{
    /// <summary>
    /// 缓存故障是否只记日志（当作未命中）而不抛出；默认开启
    /// </summary>
    public bool HideErrors { get; set; } = true;

    /// <summary>
    /// 调用方没给过期策略时的默认值；默认绝对过期 1 小时，避免键无界堆积
    /// </summary>
    public CacheEntryOptions DefaultEntryOptions { get; set; } = CacheEntryOptions.Expires(TimeSpan.FromHours(1));
}
