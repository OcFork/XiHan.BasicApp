// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.OAuthApp;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("o-auth-app.regenerate-secret", Permission = SaasPermissionCodes.OAuthApp.Secret)]
public sealed class RegenerateOAuthAppSecretOperation(IOAuthAppAppService service) : IOperation<RegenerateOAuthAppSecretRequest, OAuthAppSecretDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OAuthAppSecretDto>> HandleAsync(RegenerateOAuthAppSecretRequest request, InvocationContext context)
        => await service.RegenerateOAuthAppSecretAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record RegenerateOAuthAppSecretRequest(long Id);
