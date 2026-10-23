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
[Operation("chat.set-member-role", Permission = ChatPermissionCodes.Manage)]
public sealed class SetMemberRoleOperation(IChatAppService service) : IOperation<ChatMemberRoleDto, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(ChatMemberRoleDto request, InvocationContext context)
    {
        await service.SetMemberRoleAsync(request, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}
