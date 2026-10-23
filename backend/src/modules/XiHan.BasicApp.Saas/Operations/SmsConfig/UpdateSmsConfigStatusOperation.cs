// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.SmsConfig;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("sms-config.update-status", Permission = SaasPermissionCodes.SmsConfig.Status)]
public sealed class UpdateSmsConfigStatusOperation(ISmsConfigAppService service) : IOperation<SmsConfigStatusUpdateDto, SmsConfigDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<SmsConfigDetailDto>> HandleAsync(SmsConfigStatusUpdateDto request, InvocationContext context)
        => await service.UpdateSmsConfigStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
