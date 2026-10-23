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
[Operation("permission-condition-insight-query.get-condition-attributes", Permission = SaasPermissionCodes.PermissionCondition.Read)]
public sealed class GetConditionAttributesOperation(PermissionConditionInsightQueryService service) : IOperation<GetConditionAttributesRequest, List<PermissionConditionAttributeDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<PermissionConditionAttributeDto>>> HandleAsync(GetConditionAttributesRequest request, InvocationContext context)
        => await service.GetConditionAttributesAsync(request.PermissionCode).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetConditionAttributesRequest(string? PermissionCode = null);
