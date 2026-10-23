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
[Operation("auth.stop-impersonation", Policy = "authenticated")]
public sealed class StopImpersonationOperation(AuthAppService service) : IOperation<StopImpersonationRequest, LoginTokenDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<LoginTokenDto>> HandleAsync(StopImpersonationRequest request, InvocationContext context)
        => await service.StopImpersonationAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record StopImpersonationRequest();
