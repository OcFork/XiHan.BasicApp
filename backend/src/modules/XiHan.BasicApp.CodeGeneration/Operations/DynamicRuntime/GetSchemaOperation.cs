// 由 xihan migrate v4 生成；提交前请检查。

using SqlSugar;
using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Domain.Repositories;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.DynamicRuntime;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("dynamic-runtime.get-schema", Permission = CodeGenPermissionCodes.Read)]
public sealed class GetSchemaOperation(IDynamicRuntimeAppService service) : IOperation<GetSchemaRequest, DynamicRuntimeSchemaDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<DynamicRuntimeSchemaDto>> HandleAsync(GetSchemaRequest request, InvocationContext context)
        => await service.GetSchemaAsync(request.TableId, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetSchemaRequest(long TableId);
