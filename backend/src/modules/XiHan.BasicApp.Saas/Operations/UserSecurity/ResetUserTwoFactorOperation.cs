// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserSecurity;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-security.reset-user-two-factor", Permission = SaasPermissionCodes.UserSecurity.ResetTwoFactor)]
public sealed class ResetUserTwoFactorOperation(IUserSecurityAppService service) : IOperation<UserTwoFactorResetDto, UserSecurityDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<UserSecurityDetailDto>> HandleAsync(UserTwoFactorResetDto request, InvocationContext context)
        => await service.ResetUserTwoFactorAsync(request, context.CancellationToken).ConfigureAwait(false);
}
