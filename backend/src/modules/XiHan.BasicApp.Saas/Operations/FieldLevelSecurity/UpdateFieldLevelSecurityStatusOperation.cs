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
[Operation("field-level-security.update-status", Permission = SaasPermissionCodes.FieldLevelSecurity.Status)]
public sealed class UpdateFieldLevelSecurityStatusOperation(IFieldLevelSecurityAppService service) : IOperation<FieldLevelSecurityStatusUpdateDto, FieldLevelSecurityDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<FieldLevelSecurityDetailDto>> HandleAsync(FieldLevelSecurityStatusUpdateDto request, InvocationContext context)
        => await service.UpdateFieldLevelSecurityStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
