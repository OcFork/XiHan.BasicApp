// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices.Monitoring;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Cache;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("cache.get-string", Permission = SaasPermissionCodes.Cache.Read)]
public sealed class GetStringOperation(CacheAppService service) : IOperation<GetStringRequest, string?>
{
    /// <inheritdoc />
    public ValueTask<Result<string?>> HandleAsync(GetStringRequest request, InvocationContext context)
        => new(Result.Success(service.GetString(request.Key)));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetStringRequest(string Key);
