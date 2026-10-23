// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Operation;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("operation.update", Permission = SaasPermissionCodes.Operation.Update)]
public sealed class UpdateOperationOperation(IOperationAppService service) : IOperation<OperationUpdateDto, OperationDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OperationDetailDto>> HandleAsync(OperationUpdateDto request, InvocationContext context)
        => await service.UpdateOperationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
