// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.get-sessions", Policy = "authenticated")]
public sealed class GetSessionsOperation(ProfileAppService service) : IOperation<GetSessionsRequest, List<ProfileSessionDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<ProfileSessionDto>>> HandleAsync(GetSessionsRequest request, InvocationContext context)
        => await service.GetSessionsAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetSessionsRequest();
