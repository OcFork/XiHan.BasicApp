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
[Operation("open-api.echo")]
public sealed class EchoOperation(IOpenApiAppService service) : IOperation<OpenApiEchoInputDto, OpenApiEchoDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OpenApiEchoDto>> HandleAsync(OpenApiEchoInputDto request, InvocationContext context)
        => await service.EchoAsync(request, context.CancellationToken).ConfigureAwait(false);
}
