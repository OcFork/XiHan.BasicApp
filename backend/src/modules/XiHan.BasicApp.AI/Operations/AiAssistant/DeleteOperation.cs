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

namespace XiHan.BasicApp.AI.Operations.AiAssistant;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("ai-assistant.delete", Permission = AiAssistantPermissionCodes.Delete)]
public sealed class DeleteOperation(IAiAssistantAppService service) : IOperation<DeleteRequest, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(DeleteRequest request, InvocationContext context)
    {
        await service.DeleteAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record DeleteRequest(long Id);
