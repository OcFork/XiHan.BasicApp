// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System.Text;
using XiHan.BasicApp.AI.Application.AppServices;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Domain.Entities;
using XiHan.BasicApp.AI.Domain.Repositories;
using XiHan.BasicApp.AI.Operations;
using XiHan.BasicApp.Chat.Application.Mappers;
using XiHan.BasicApp.Chat.Application.Services;
using XiHan.BasicApp.Chat.Domain.DomainServices;
using XiHan.BasicApp.Chat.Domain.Entities;
using XiHan.BasicApp.Chat.Domain.Permissions;
using XiHan.BasicApp.Chat.Domain.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.AI.Operations.ChatAssistant;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("chat-assistant.open-conversation", Permission = ChatPermissionCodes.Send)]
public sealed class OpenConversationOperation(IChatAssistantAppService service) : IOperation<ChatAssistantOpenDto, ChatAssistantConversationDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ChatAssistantConversationDto>> HandleAsync(ChatAssistantOpenDto request, InvocationContext context)
        => await service.OpenConversationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
