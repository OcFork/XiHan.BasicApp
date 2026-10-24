// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.Primitives;
using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Saas.Application.Authorization;

/// <summary>
/// 授权变更通知器实现
/// </summary>
public sealed class AuthorizationChangeNotifier : IAuthorizationChangeNotifier
{
    private readonly IMessagePublisher _localEventBus;

    private readonly ICurrentUser _currentUser;

    private readonly ICurrentTenant _currentTenant;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AuthorizationChangeNotifier(
        IMessagePublisher localEventBus,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant)
    {
        _localEventBus = localEventBus;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
    }

    /// <summary>
    /// 发布一条授权变更事件。
    /// </summary>
    /// <param name="changeType">变更类型</param>
    /// <param name="targetUserId">目标用户ID（用户级变更时填写）</param>
    /// <param name="targetRoleId">目标角色ID（角色级变更、或用户分配/移除角色时填写）</param>
    /// <param name="permissionId">权限ID（权限级变更时填写）</param>
    /// <param name="reason">变更原因</param>
    /// <param name="relatedRoleId">关联角色ID（角色新增 / 解除上级时为上级角色）</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task NotifyAsync(
        PermissionChangeType changeType,
        long? targetUserId,
        long? targetRoleId,
        long? permissionId,
        string? reason = null,
        long? relatedRoleId = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var domainEvent = new AuthorizationChangedDomainEvent(
            TenantIds.Normalize(_currentTenant.Id),
            changeType,
            targetUserId,
            targetRoleId,
            permissionId,
            _currentUser.UserId,
            reason,
            relatedRoleId);

        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        return _localEventBus.PublishAsync("authorization-changed", new MessageEnvelope { Id = Guid.NewGuid().ToString("N"), Source = "/saas", Type = "authorization-changed", Time = DateTimeOffset.UtcNow, Data = JsonSerializer.SerializeToUtf8Bytes(domainEvent) }, cancellationToken).AsTask();
    }
}
