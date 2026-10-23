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
using XiHan.Framework.Utils.Reflections;

namespace XiHan.BasicApp.Saas.Operations.Server;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("server.get-nu-get-packages", Permission = SaasPermissionCodes.Server.Read)]
public sealed class GetNuGetPackagesOperation(ServerAppService service) : IOperation<GetNuGetPackagesRequest, List<NuGetPackage>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<NuGetPackage>>> HandleAsync(GetNuGetPackagesRequest request, InvocationContext context)
        => await service.GetNuGetPackagesAsync().ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetNuGetPackagesRequest();
