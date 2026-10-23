// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserSession;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-session.revoke-user-sessions", Permission = SaasPermissionCodes.UserSession.Revoke)]
public sealed class RevokeUserSessionsOperation(IUserSessionAppService service) : IOperation<UserSessionsRevokeDto, int>
{
    /// <inheritdoc />
    public async ValueTask<Result<int>> HandleAsync(UserSessionsRevokeDto request, InvocationContext context)
        => await service.RevokeUserSessionsAsync(request, context.CancellationToken).ConfigureAwait(false);
}
