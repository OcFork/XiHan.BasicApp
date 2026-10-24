// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Security.Claims;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 5.0 主体（<see cref="Principal"/>，键值声明）与 4.x 承接代码使用的 <see cref="ClaimsPrincipal"/> 互转
/// </summary>
public static class PrincipalConversion
{
    /// <summary>
    /// JWT 里的主体声明名
    /// </summary>
    private const string SubjectClaim = "sub";

    /// <summary>
    /// JwtSecurityTokenHandler 对 <see cref="ClaimTypes.NameIdentifier"/> 的出站短名
    /// </summary>
    private const string NameIdShortClaim = "nameid";

    /// <summary>
    /// 没有认证方案信息时给覆盖主体的方案名
    /// </summary>
    private const string AmbientScheme = "ambient";

    /// <summary>
    /// 5.0 主体转 ClaimsPrincipal：声明按原名复制，认证类型取认证方案；匿名主体给空标识
    /// </summary>
    /// <param name="principal">5.0 主体</param>
    /// <returns>ClaimsPrincipal</returns>
    public static ClaimsPrincipal ToClaimsPrincipal(Principal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        if (!principal.IsAuthenticated)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var identity = new ClaimsIdentity(principal.Claims.Select(static claim => new Claim(claim.Key, claim.Value)), principal.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    /// <summary>
    /// ClaimsPrincipal 转 5.0 主体：标识依次取用户 Id 声明（<see cref="XiHanClaimTypes.UserId"/>）、<c>sub</c>、<c>nameid</c>；
    /// 取不到标识视为匿名
    /// </summary>
    /// <param name="principal">ClaimsPrincipal</param>
    /// <returns>5.0 主体</returns>
    public static Principal FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        var id = principal.FindFirst(XiHanClaimTypes.UserId)?.Value
            ?? principal.FindFirst(SubjectClaim)?.Value
            ?? principal.FindFirst(NameIdShortClaim)?.Value;
        if (string.IsNullOrWhiteSpace(id))
        {
            return Principal.Anonymous;
        }

        var scheme = principal.Identity?.AuthenticationType is { Length: > 0 } authenticationType ? authenticationType : AmbientScheme;
        return new Principal(id, scheme)
        {
            DisplayName = principal.Identity?.Name,
            Claims = [.. principal.Claims.Select(static claim => new KeyValuePair<string, string>(claim.Type, claim.Value))],
        };
    }
}
