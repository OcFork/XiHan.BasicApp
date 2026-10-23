// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Identity;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.MyOAuthApp;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("my-o-auth-app.regenerate-secret", Policy = "authenticated")]
public sealed class RegenerateMyOAuthAppSecretOperation(IMyOAuthAppAppService service) : IOperation<RegenerateMyOAuthAppSecretRequest, MyOAuthAppSecretDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<MyOAuthAppSecretDto>> HandleAsync(RegenerateMyOAuthAppSecretRequest request, InvocationContext context)
        => await service.RegenerateMyOAuthAppSecretAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record RegenerateMyOAuthAppSecretRequest(long Id);
