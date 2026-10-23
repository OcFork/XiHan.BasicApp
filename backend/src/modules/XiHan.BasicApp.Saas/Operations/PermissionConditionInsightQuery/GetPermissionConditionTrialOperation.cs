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
[Operation("permission-condition-insight-query.get-permission-condition-trial", Permission = SaasPermissionCodes.PermissionCondition.Trial)]
public sealed class GetPermissionConditionTrialOperation(PermissionConditionInsightQueryService service) : IOperation<PermissionConditionTrialInputDto, PermissionConditionTrialResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionConditionTrialResultDto>> HandleAsync(PermissionConditionTrialInputDto request, InvocationContext context)
        => await service.GetPermissionConditionTrialAsync(request, context.CancellationToken).ConfigureAwait(false);
}
