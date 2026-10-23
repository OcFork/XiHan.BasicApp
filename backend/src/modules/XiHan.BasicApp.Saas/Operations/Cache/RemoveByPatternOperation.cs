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
[Operation("cache.delete-by-pattern", Permission = SaasPermissionCodes.Cache.Clear)]
public sealed class RemoveByPatternOperation(CacheAppService service) : IOperation<RemoveByPatternRequest, long>
{
    /// <inheritdoc />
    public ValueTask<Result<long>> HandleAsync(RemoveByPatternRequest request, InvocationContext context)
        => new(Result.Success(service.RemoveByPattern(request.Pattern)));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record RemoveByPatternRequest(string Pattern = "*");
