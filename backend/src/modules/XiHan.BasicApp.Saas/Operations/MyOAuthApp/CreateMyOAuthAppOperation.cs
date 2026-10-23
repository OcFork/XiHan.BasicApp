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
[Operation("my-o-auth-app.create", Policy = "authenticated")]
public sealed class CreateMyOAuthAppOperation(IMyOAuthAppAppService service) : IOperation<MyOAuthAppCreateDto, MyOAuthAppSecretDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<MyOAuthAppSecretDto>> HandleAsync(MyOAuthAppCreateDto request, InvocationContext context)
        => await service.CreateMyOAuthAppAsync(request, context.CancellationToken).ConfigureAwait(false);
}
