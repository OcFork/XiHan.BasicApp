// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.Framework.Messaging.Abstractions.Envelopes;
using XiHan.Framework.Messaging.Abstractions.Publishing;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Application.AppServices;

/// <summary>
/// 用户会话命令应用服务
/// </summary>
public sealed class UserSessionAppService
    : SaasApplicationService, IUserSessionAppService
{
    private readonly ICurrentUser _currentUser;

    private readonly IMessagePublisher _localEventBus;

    private readonly IUserDomainService _userDomainService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public UserSessionAppService(
        IUserDomainService userDomainService,
        ICurrentUser currentUser,
        IMessagePublisher localEventBus)
    {
        _userDomainService = userDomainService;
        _currentUser = currentUser;
        _localEventBus = localEventBus;
    }

    #region 用户会话

    /// <summary>
    /// 撤销用户会话
    /// </summary>
    public async Task<UserSessionDetailDto> RevokeUserSessionAsync(UserSessionRevokeDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _userDomainService.RevokeUserSessionAsync(
            UserSessionApplicationMapper.ToRevokeCommand(input, _currentUser.UserId),
            cancellationToken);
        await PublishDomainEventAsync(result.DomainEvent, cancellationToken);
        return UserSessionApplicationMapper.ToDetailDto(result.Session, result.User, result.Now);
    }

    /// <summary>
    /// 撤销用户全部会话
    /// </summary>
    public async Task<int> RevokeUserSessionsAsync(UserSessionsRevokeDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _userDomainService.RevokeUserSessionsAsync(
            UserSessionApplicationMapper.ToRevokeAllCommand(input, _currentUser.UserId),
            cancellationToken);
        await PublishDomainEventAsync(result.DomainEvent, cancellationToken);
        return result.Count;
    }

    #endregion

    private async Task PublishDomainEventAsync(UserSessionRevokedDomainEvent? domainEvent, CancellationToken cancellationToken)
    {
        if (domainEvent is null)
        {
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();
        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await _localEventBus.PublishAsync("user-session-revoked", new MessageEnvelope { Id = Guid.NewGuid().ToString("N"), Source = "/saas", Type = "user-session-revoked", Time = DateTimeOffset.UtcNow, Data = JsonSerializer.SerializeToUtf8Bytes(domainEvent) }, cancellationToken);
    }
}
