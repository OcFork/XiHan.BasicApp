// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Authorization;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.PermissionCondition;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("permission-condition.update", Permission = SaasPermissionCodes.PermissionCondition.Update)]
public sealed class UpdatePermissionConditionOperation(IPermissionConditionAppService service) : IOperation<PermissionConditionUpdateDto, PermissionConditionDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionConditionDetailDto>> HandleAsync(PermissionConditionUpdateDto request, InvocationContext context)
        => await service.UpdatePermissionConditionAsync(request, context.CancellationToken).ConfigureAwait(false);
}
