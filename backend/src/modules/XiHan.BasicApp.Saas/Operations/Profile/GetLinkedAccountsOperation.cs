// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.get-linked-accounts", Policy = "authenticated")]
public sealed class GetLinkedAccountsOperation(IProfileAppService service) : IOperation<GetLinkedAccountsRequest, List<ProfileExternalLoginDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<ProfileExternalLoginDto>>> HandleAsync(GetLinkedAccountsRequest request, InvocationContext context)
        => await service.GetLinkedAccountsAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetLinkedAccountsRequest();
