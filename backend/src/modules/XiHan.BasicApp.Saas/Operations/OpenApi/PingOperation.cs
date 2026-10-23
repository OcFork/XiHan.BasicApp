// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Saas.Application;
using XiHan.BasicApp.Saas.OpenApi.AppServices;
using XiHan.BasicApp.Saas.OpenApi.Contracts;
using XiHan.BasicApp.Saas.OpenApi.Dtos;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.OpenApi;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("open-api.ping")]
public sealed class PingOperation(IOpenApiAppService service) : IOperation<PingRequest, OpenApiPingDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OpenApiPingDto>> HandleAsync(PingRequest request, InvocationContext context)
        => await service.PingAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record PingRequest();
