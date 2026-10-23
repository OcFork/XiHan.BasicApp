// 由 xihan migrate v4 生成；提交前请检查。

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

namespace XiHan.BasicApp.AI.Operations.AiPrompt;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("ai-prompt.update-status", Permission = AiPromptPermissionCodes.Update)]
public sealed class UpdateStatusOperation(IAiPromptAppService service) : IOperation<AiPromptStatusUpdateDto, AiPromptDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<AiPromptDetailDto>> HandleAsync(AiPromptStatusUpdateDto request, InvocationContext context)
        => await service.UpdateStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
