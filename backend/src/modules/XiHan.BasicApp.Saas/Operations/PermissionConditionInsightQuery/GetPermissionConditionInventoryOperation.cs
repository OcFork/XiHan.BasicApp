// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Saas.Application.Conditions;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Conditions;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.PermissionConditionInsightQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("permission-condition-insight-query.get-permission-condition-inventory", Permission = SaasPermissionCodes.PermissionCondition.Trial)]
public sealed class GetPermissionConditionInventoryOperation(PermissionConditionInsightQueryService service) : IOperation<GetPermissionConditionInventoryRequest, PermissionConditionInventoryDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionConditionInventoryDto>> HandleAsync(GetPermissionConditionInventoryRequest request, InvocationContext context)
        => await service.GetPermissionConditionInventoryAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetPermissionConditionInventoryRequest();
