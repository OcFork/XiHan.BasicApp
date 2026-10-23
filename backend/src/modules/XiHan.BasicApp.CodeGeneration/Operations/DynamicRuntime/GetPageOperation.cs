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
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.DynamicRuntime;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("dynamic-runtime.list", Permission = CodeGenPermissionCodes.Read)]
public sealed class GetPageOperation(IDynamicRuntimeAppService service) : IOperation<DynamicRuntimePageQueryDto, DynamicRuntimePageResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<DynamicRuntimePageResultDto>> HandleAsync(DynamicRuntimePageQueryDto request, InvocationContext context)
        => await service.GetPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
