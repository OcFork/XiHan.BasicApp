// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 默认缓存键规范化：<c>{租户}:{缓存名}:{键}</c>
/// </summary>
/// <param name="currentTenantAccessor">当前租户访问器（单例，从执行流环境读租户，所以本实现与架在它上面的缓存分区都能是单例，供后台单例服务使用）</param>
public sealed class DefaultDistributedCacheKeyNormalizer(ICurrentTenantAccessor currentTenantAccessor) : IDistributedCacheKeyNormalizer
{
    /// <inheritdoc />
    public string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        // 租户段用稳定不变的租户标识，平台与无租户上下文都是 "0"
        var tenantSegment = args.IgnoreMultiTenancy ? TenantIds.Platform : TenantIds.Normalize(currentTenantAccessor.Current?.TenantId);
        return $"{tenantSegment}:{args.CacheName}:{args.Key}";
    }
}
