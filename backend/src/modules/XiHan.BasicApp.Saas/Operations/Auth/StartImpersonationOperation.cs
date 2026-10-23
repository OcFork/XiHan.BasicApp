// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Configurations;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Auth;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("auth.start-impersonation", Permission = SaasPermissionCodes.Impersonation.Start)]
public sealed class StartImpersonationOperation(AuthAppService service) : IOperation<StartImpersonationRequestDto, LoginTokenDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<LoginTokenDto>> HandleAsync(StartImpersonationRequestDto request, InvocationContext context)
        => await service.StartImpersonationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
