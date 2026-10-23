// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XiHan.BasicApp.Chat.Application.Contracts;
using XiHan.BasicApp.Chat.Application.Dtos;
using XiHan.BasicApp.Chat.Application.QueryServices;
using XiHan.BasicApp.Chat.Domain.DomainServices;
using XiHan.BasicApp.Chat.Domain.Entities;
using XiHan.BasicApp.Chat.Domain.Permissions;
using XiHan.BasicApp.Chat.Operations;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Chat.Operations.ChatAuditQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("chat-audit-query.list-chat-message", Permission = ChatPermissionCodes.Audit)]
public sealed class GetChatMessagePageOperation(IChatAuditQueryService service) : IOperation<ChatAuditPageQueryDto, PageResultDtoBase<ChatAuditListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<ChatAuditListItemDto>>> HandleAsync(ChatAuditPageQueryDto request, InvocationContext context)
        => await service.GetChatMessagePageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
