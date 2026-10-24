// 由 xihan migrate v4 生成；提交前请检查。

using System.Security.Claims;
using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed class CurrentUser(IInvocationAccessor accessor) : ICurrentUser
{
    private const string EmailVerifiedClaim = "email_verified";
    private const string PhoneNumberClaim = "phone_number";
    private const string PhoneNumberVerifiedClaim = "phone_number_verified";
    private const string TenantIdClaim = "tenantid";

    private SecurityContext Security => accessor.Current.TryGetValue(out var context) ? context.GetSecurity() : SecurityContext.Anonymous;

    private Principal Principal => Security.Principal;

    /// <inheritdoc />
    public bool IsAuthenticated => Principal.IsAuthenticated;

    /// <inheritdoc />
    public string? Subject => Principal.IsAuthenticated ? Principal.Id : null;

    /// <inheritdoc />
    // 本应用的用户主键是 long，主体标识就是它的字符串形式；解析不出数字视为未登录用户
    public long? UserId => long.TryParse(Subject, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var userId) ? userId : null;

    /// <inheritdoc />
    public string? UserName => Find(ClaimTypes.Name);

    /// <inheritdoc />
    public string? Name => Find(ClaimTypes.GivenName);

    /// <inheritdoc />
    public string? SurName => Find(ClaimTypes.Surname);

    /// <inheritdoc />
    public string? PhoneNumber => Find(PhoneNumberClaim);

    /// <inheritdoc />
    public bool PhoneNumberVerified => string.Equals(Find(PhoneNumberVerifiedClaim), "true", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public string? Email => Find(ClaimTypes.Email);

    /// <inheritdoc />
    public bool EmailVerified => string.Equals(Find(EmailVerifiedClaim), "true", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public string? TenantId => Security.Tenant?.Id ?? Find(TenantIdClaim);

    /// <inheritdoc />
    public string[] Roles => [.. Principal.ClaimValues(ClaimTypes.Role).Distinct(StringComparer.Ordinal)];

    /// <inheritdoc />
    public Claim? FindClaim(string claimType) => GetAllClaims().FirstOrDefault(claim => claim.Type == claimType);

    /// <inheritdoc />
    public Claim[] FindClaims(string claimType) => [.. GetAllClaims().Where(claim => claim.Type == claimType)];

    /// <inheritdoc />
    public Claim[] GetAllClaims() => [.. Principal.Claims.Select(claim => new Claim(claim.Key, claim.Value))];

    /// <inheritdoc />
    public bool IsInRole(string roleName) => Principal.HasClaim(ClaimTypes.Role, roleName);

    private string? Find(string claimType) => Principal.ClaimValues(claimType).FirstOrDefault();
}
