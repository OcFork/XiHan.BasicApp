// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;
using XiHan.BasicApp.Core.Auditing.Writers;
using XiHan.BasicApp.Core.Auditing;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Web.Core.Clients;

namespace XiHan.BasicApp.Saas.Infrastructure.Logging;

/// <summary>
/// SaaS 登录日志写入器
/// </summary>
public class SaasLoginLogWriter : ILoginLogWriter
{
    private readonly ISqlSugarClientResolver _clientResolver;
    private readonly ICurrentTenant _currentTenant;
    private readonly IClientInfoProvider _clientInfoProvider;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// 构造函数
    /// </summary>
    public SaasLoginLogWriter(
        ISqlSugarClientResolver clientResolver,
        ICurrentTenant currentTenant,
        IClientInfoProvider clientInfoProvider,
        ICurrentUser currentUser)
    {
        _clientResolver = clientResolver;
        _currentTenant = currentTenant;
        _clientInfoProvider = clientInfoProvider;
        _currentUser = currentUser;
    }

    private ISqlSugarClient DbClient => _clientResolver.GetCurrentClient();

    /// <summary>
    /// 写入登录日志
    /// </summary>
    public async Task WriteAsync(LoginLogRecord record, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(record);

        var clientInfo = _clientInfoProvider.GetCurrent();
        // 落在记录产生时的租户，而不是写入时的环境上下文（位于租户解析之前的中间件、排队异步写入时环境里都没有请求的租户）；
        // 切入该租户再取连接，库隔离租户的日志进它自己的库
        // xihan-migrate(V4-07): ICurrentTenant.Change 没有 5.0 对应物：租户随调用的 Baggage 走；改为带租户 Baggage 发起新调用（App.CreateContext）。
        using var tenantScope = _currentTenant.Change(record.TenantId);
        var tenantId = record.TenantId ?? 0;

        var entity = new SysLoginLog
        {
            TenantId = tenantId,
            UserId = record.UserId,
            UserName = SaasLogMappingHelper.TrimOrNull(record.UserName, 50),
            SessionId = SaasLogMappingHelper.TrimOrNull(record.SessionId, 100),
            TraceId = SaasLogMappingHelper.TrimOrNull(record.TraceId, 64),
            LoginIp = SaasLogMappingHelper.TrimOrNull(record.LoginIp ?? clientInfo.IpAddress, 50),
            LoginLocation = SaasLogMappingHelper.TrimOrNull(clientInfo.Location, 200),
            Browser = SaasLogMappingHelper.TrimOrNull(clientInfo.Browser, 100),
            Os = SaasLogMappingHelper.TrimOrNull(clientInfo.OperatingSystem, 100),
            UserAgent = SaasLogMappingHelper.TrimOrNull(record.UserAgent ?? clientInfo.UserAgent, 500),
            Device = SaasLogMappingHelper.TrimOrNull(clientInfo.DeviceName, 50),
            // 登录/失败由事件显式带入；登出、切租户、令牌刷新等已认证路径没有入参，
            // 从令牌的设备指纹声明补全（签发时由 AuthTokenIssueService 写入）
            DeviceId = SaasLogMappingHelper.TrimOrNull(
                record.DeviceId ?? _currentUser.FindClaim(XiHanClaimTypes.DeviceFingerprint)?.Value,
                200),
            LoginResult = (LoginResult)record.LoginResult,
            Message = SaasLogMappingHelper.TrimOrNull(record.Message, 500),
            LoginTime = record.LoginTime,
            CreatedAt = record.LoginTime
        };

        await DbClient.Insertable(entity).SplitTable().ExecuteCommandAsync();
    }
}
