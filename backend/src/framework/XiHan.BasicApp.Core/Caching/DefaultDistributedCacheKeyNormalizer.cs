// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 默认键规范化：<c>{租户}:{缓存名}:{业务键}</c>，忽略多租户的缓存项租户段固定为平台
/// </summary>
/// <param name="currentTenant">当前租户</param>
public sealed class DefaultDistributedCacheKeyNormalizer(ICurrentTenant currentTenant) : IDistributedCacheKeyNormalizer
{
    /// <inheritdoc />
    public string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        // 租户段用稳定不变的租户标识，平台与无租户上下文都是 "0"
        var tenantSegment = args.IgnoreMultiTenancy ? TenantIds.Platform : TenantIds.Normalize(currentTenant.Id);
        return $"{tenantSegment}:{args.CacheName}:{args.Key}";
    }
}
