// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Chat.Application.AppServices;
using XiHan.BasicApp.Chat.Application.Contracts;
using XiHan.BasicApp.Chat.Application.Dtos;
using XiHan.BasicApp.Chat.Application.Mappers;
using XiHan.BasicApp.Chat.Application.Services;
using XiHan.BasicApp.Chat.Domain.DomainServices;
using XiHan.BasicApp.Chat.Domain.Permissions;
using XiHan.BasicApp.Chat.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Chat.Operations.Chat;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("chat.toggle-pin-conversation", Permission = ChatPermissionCodes.Read)]
public sealed class TogglePinConversationOperation(IChatAppService service) : IOperation<ChatConversationToggleDto, ChatToggleStateDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ChatToggleStateDto>> HandleAsync(ChatConversationToggleDto request, InvocationContext context)
        => await service.TogglePinConversationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
