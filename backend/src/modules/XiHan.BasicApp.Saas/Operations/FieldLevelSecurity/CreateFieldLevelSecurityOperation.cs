// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.FieldLevelSecurity;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("field-level-security.create", Permission = SaasPermissionCodes.FieldLevelSecurity.Create)]
public sealed class CreateFieldLevelSecurityOperation(IFieldLevelSecurityAppService service) : IOperation<FieldLevelSecurityCreateDto, FieldLevelSecurityDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<FieldLevelSecurityDetailDto>> HandleAsync(FieldLevelSecurityCreateDto request, InvocationContext context)
        => await service.CreateFieldLevelSecurityAsync(request, context.CancellationToken).ConfigureAwait(false);
}
