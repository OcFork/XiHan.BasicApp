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
[Operation("cache.update-string", Permission = SaasPermissionCodes.Cache.Clear)]
public sealed class UpdateStringOperation(CacheAppService service) : IOperation<CacheStringUpdateDto, EmptyResponse>
{
    /// <inheritdoc />
    public ValueTask<Result<EmptyResponse>> HandleAsync(CacheStringUpdateDto request, InvocationContext context)
    {
        service.UpdateString(request);
        return new(Result.Success(EmptyResponse.Instance));
    }
}
