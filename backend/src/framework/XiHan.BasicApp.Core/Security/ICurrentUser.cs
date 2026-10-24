// 由 xihan migrate v4 生成；提交前请检查。

using System.Security.Claims;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    long? UserId { get; }

    /// <summary>
    /// 5.0 主体标识原文（本应用里是用户 Id 的字符串形式）；未登录为空
    /// </summary>
    string? Subject { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? SurName { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? PhoneNumber { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    bool PhoneNumberVerified { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    bool EmailVerified { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? TenantId { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string[] Roles { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    Claim? FindClaim(string claimType);

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    Claim[] FindClaims(string claimType);

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    Claim[] GetAllClaims();

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    bool IsInRole(string roleName);
}
