// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.DepartmentManagementQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("department-management-query.get-department-management-detail", Permission = SaasPermissionCodes.Department.Read)]
public sealed class GetDepartmentManagementDetailOperation(IDepartmentManagementQueryService service) : IOperation<GetDepartmentManagementDetailRequest, DepartmentManagementDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<DepartmentManagementDetailDto?>> HandleAsync(GetDepartmentManagementDetailRequest request, InvocationContext context)
        => await service.GetDepartmentManagementDetailAsync(request.DepartmentId, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetDepartmentManagementDetailRequest(long DepartmentId);
