// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.AI.Application.AppServices;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Application.Mappers;
using XiHan.BasicApp.AI.Domain.DomainServices;
using XiHan.BasicApp.AI.Domain.Entities;
using XiHan.BasicApp.AI.Domain.Permissions;
using XiHan.BasicApp.AI.Operations;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.AI.Operations.AiProvider;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("ai-provider.set-default", Permission = AiPermissionCodes.Update)]
public sealed class SetDefaultOperation(IAiProviderAppService service) : IOperation<AiProviderActionDto, AiProviderDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<AiProviderDetailDto>> HandleAsync(AiProviderActionDto request, InvocationContext context)
        => await service.SetDefaultAsync(request, context.CancellationToken).ConfigureAwait(false);
}
