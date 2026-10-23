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
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.AI.Operations.ChatAssistant;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("chat-assistant.reply", Permission = ChatPermissionCodes.Send)]
public sealed class ReplyOperation(IChatAssistantAppService service) : IOperation<ChatAssistantReplyDto, ChatAssistantReplyResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ChatAssistantReplyResultDto>> HandleAsync(ChatAssistantReplyDto request, InvocationContext context)
        => await service.ReplyAsync(request, context.CancellationToken).ConfigureAwait(false);
}
