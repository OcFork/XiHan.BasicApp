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

namespace XiHan.BasicApp.Saas.Operations.Department;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("department.create", Permission = SaasPermissionCodes.Department.Create)]
public sealed class CreateDepartmentOperation(IDepartmentAppService service) : IOperation<DepartmentCreateDto, DepartmentDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<DepartmentDetailDto>> HandleAsync(DepartmentCreateDto request, InvocationContext context)
        => await service.CreateDepartmentAsync(request, context.CancellationToken).ConfigureAwait(false);
}
