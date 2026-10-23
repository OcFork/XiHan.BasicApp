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
[Operation("profile.get-login-logs", Policy = "authenticated")]
public sealed class GetLoginLogsOperation(ProfileAppService service) : IOperation<GetLoginLogsRequest, ProfileLoginLogPageDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileLoginLogPageDto>> HandleAsync(GetLoginLogsRequest request, InvocationContext context)
        => await service.GetLoginLogsAsync(request.Page, request.PageSize, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetLoginLogsRequest(int Page = 1, int PageSize = 10);
