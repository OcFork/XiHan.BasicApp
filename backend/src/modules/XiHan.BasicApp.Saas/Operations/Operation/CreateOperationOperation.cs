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
[Operation("operation.create", Permission = SaasPermissionCodes.Operation.Create)]
public sealed class CreateOperationOperation(IOperationAppService service) : IOperation<OperationCreateDto, OperationDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OperationDetailDto>> HandleAsync(OperationCreateDto request, InvocationContext context)
        => await service.CreateOperationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
