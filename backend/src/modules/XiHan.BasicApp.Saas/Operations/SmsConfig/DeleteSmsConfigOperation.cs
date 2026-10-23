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
[Operation("sms-config.delete", Permission = SaasPermissionCodes.SmsConfig.Delete)]
public sealed class DeleteSmsConfigOperation(ISmsConfigAppService service) : IOperation<DeleteSmsConfigRequest, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(DeleteSmsConfigRequest request, InvocationContext context)
    {
        await service.DeleteSmsConfigAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record DeleteSmsConfigRequest(long Id);
