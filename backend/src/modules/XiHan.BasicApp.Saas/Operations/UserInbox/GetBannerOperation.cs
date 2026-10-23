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
[Operation("user-inbox.get-banner", Policy = "authenticated")]
public sealed class GetBannerOperation(IUserInboxAppService service) : IOperation<GetBannerRequest, List<UserInboxItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<UserInboxItemDto>>> HandleAsync(GetBannerRequest request, InvocationContext context)
        => await service.GetBannerAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetBannerRequest();
