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
[Operation("permission-condition-insight-query.get-permission-condition-batch-trial", Permission = SaasPermissionCodes.PermissionCondition.Trial)]
public sealed class GetPermissionConditionBatchTrialOperation(PermissionConditionInsightQueryService service) : IOperation<PermissionConditionBatchTrialInputDto, PermissionConditionBatchTrialResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionConditionBatchTrialResultDto>> HandleAsync(PermissionConditionBatchTrialInputDto request, InvocationContext context)
        => await service.GetPermissionConditionBatchTrialAsync(request, context.CancellationToken).ConfigureAwait(false);
}
