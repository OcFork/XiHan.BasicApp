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
[Operation("my-o-auth-app.delete", Policy = "authenticated")]
public sealed class DeleteMyOAuthAppOperation(IMyOAuthAppAppService service) : IOperation<DeleteMyOAuthAppRequest, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(DeleteMyOAuthAppRequest request, InvocationContext context)
    {
        await service.DeleteMyOAuthAppAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record DeleteMyOAuthAppRequest(long Id);
