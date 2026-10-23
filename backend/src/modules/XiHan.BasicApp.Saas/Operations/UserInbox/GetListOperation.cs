// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserInbox;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-inbox.list", Policy = "authenticated")]
public sealed class GetListOperation(IUserInboxAppService service) : IOperation<GetListRequest, List<UserInboxItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<UserInboxItemDto>>> HandleAsync(GetListRequest request, InvocationContext context)
        => await service.GetListAsync(request.UnreadOnly, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetListRequest(bool UnreadOnly = false);
