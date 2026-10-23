// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices.Monitoring;
using XiHan.BasicApp.Saas.Application.Dtos.Monitoring;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;
using XiHan.Framework.Utils.Diagnostics.HardwareInfos;

namespace XiHan.BasicApp.Saas.Operations.Server;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("server.get-board-info", Permission = SaasPermissionCodes.Server.Read)]
public sealed class GetBoardInfoOperation(ServerAppService service) : IOperation<GetBoardInfoRequest, BoardInfo>
{
    /// <inheritdoc />
    public async ValueTask<Result<BoardInfo>> HandleAsync(GetBoardInfoRequest request, InvocationContext context)
        => await service.GetBoardInfoAsync().ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetBoardInfoRequest();
