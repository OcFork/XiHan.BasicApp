// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.NotificationQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("notification-query.list-notification-unread-user", Permission = SaasPermissionCodes.Notification.Read)]
public sealed class GetNotificationUnreadUserPageOperation(INotificationQueryService service) : IOperation<NotificationUnreadUserPageQueryDto, PageResultDtoBase<NotificationUnreadUserDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<NotificationUnreadUserDto>>> HandleAsync(NotificationUnreadUserPageQueryDto request, InvocationContext context)
        => await service.GetNotificationUnreadUserPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
