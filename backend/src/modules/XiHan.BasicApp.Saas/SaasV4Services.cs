// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddSaasV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.AppServices.Metadata;
using XiHan.BasicApp.Saas.Application.AppServices.Monitoring;
using XiHan.BasicApp.Saas.Application.EventHandlers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.OpenApi.AppServices;
using XiHan.BasicApp.Saas.Security;
using XiHan.Framework.Messaging.Abstractions;

namespace XiHan.BasicApp.Saas;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class SaasV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddSaasV4Services(this IServiceCollection services)
    {
        services.AddScoped<IAccessPolicyAppService, AccessPolicyAppService>();
        services.AddScoped<AuthAppService>();
        services.AddScoped<IBotConfigAppService, BotConfigAppService>();
        services.AddScoped<IConfigAppService, ConfigAppService>();
        services.AddScoped<IConstraintRuleAppService, ConstraintRuleAppService>();
        services.AddScoped<IDepartmentAppService, DepartmentAppService>();
        services.AddScoped<IDictAppService, DictAppService>();
        services.AddScoped<IEmailConfigAppService, EmailConfigAppService>();
        services.AddScoped<IExportTaskAppService, ExportTaskAppService>();
        services.AddScoped<IFieldLevelSecurityAppService, FieldLevelSecurityAppService>();
        services.AddScoped<IFileAppService, FileAppService>();
        services.AddScoped<IImportHistoryAppService, ImportHistoryAppService>();
        services.AddScoped<IMenuAppService, MenuAppService>();
        services.AddScoped<IMessageAppService, MessageAppService>();
        services.AddScoped<IMessageTemplateAppService, MessageTemplateAppService>();
        services.AddScoped<IMyOAuthAppAppService, MyOAuthAppAppService>();
        services.AddScoped<INotificationAppService, NotificationAppService>();
        services.AddScoped<INumberingAppService, NumberingAppService>();
        services.AddScoped<INumberingRuleAppService, NumberingRuleAppService>();
        services.AddScoped<IOAuthAppAppService, OAuthAppAppService>();
        services.AddScoped<IOAuthConsentAppService, OAuthConsentAppService>();
        services.AddScoped<IOperationAppService, OperationAppService>();
        services.AddScoped<IPermissionAppService, PermissionAppService>();
        services.AddScoped<IPermissionConditionAppService, PermissionConditionAppService>();
        services.AddScoped<IPermissionDelegationAppService, PermissionDelegationAppService>();
        services.AddScoped<IPermissionRequestAppService, PermissionRequestAppService>();
        services.AddScoped<IPositionAppService, PositionAppService>();
        services.AddScoped<ProfileAppService>();
        services.AddScoped<IResourceAppService, ResourceAppService>();
        services.AddScoped<IRoleAppService, RoleAppService>();
        services.AddScoped<ISmsConfigAppService, SmsConfigAppService>();
        services.AddScoped<IStorageConfigAppService, StorageConfigAppService>();
        services.AddScoped<ITaskAppService, TaskAppService>();
        services.AddScoped<ITelegramBotAppService, TelegramBotAppService>();
        services.AddScoped<ITenantAppService, TenantAppService>();
        services.AddScoped<ITenantEditionAppService, TenantEditionAppService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IUserDataScopeAppService, UserDataScopeAppService>();
        services.AddScoped<IUserDepartmentAppService, UserDepartmentAppService>();
        services.AddScoped<IUserInboxAppService, UserInboxAppService>();
        services.AddScoped<IUserPermissionAppService, UserPermissionAppService>();
        services.AddScoped<IUserRoleAppService, UserRoleAppService>();
        services.AddScoped<IUserSecurityAppService, UserSecurityAppService>();
        services.AddScoped<IUserSessionAppService, UserSessionAppService>();
        services.AddScoped<IUserSettingAppService, UserSettingAppService>();
        services.AddScoped<EnumMetadataAppService>();
        services.AddScoped<TimeZoneMetadataAppService>();
        services.AddScoped<CacheAppService>();
        services.AddScoped<ServerAppService>();
        services.AddMessageHandler<AuthLoginEventHandler>(AuthLoginEventHandler.AuthLoginSucceededTopic);
        services.AddMessageHandler<AuthLoginEventHandler>(AuthLoginEventHandler.AuthLoginFailedTopic);
        services.AddMessageHandler<AuthLoginEventHandler>(AuthLoginEventHandler.AuthLogoutTopic);
        services.AddMessageHandler<AuthLoginEventHandler>(AuthLoginEventHandler.AuthSecurityAuditTopic);
        services.AddMessageHandler<AuthorizationChangedEventHandler>(AuthorizationChangedEventHandler.AuthorizationChangedTopic);
        services.AddMessageHandler<FileDeletedEventHandler>(FileDeletedEventHandler.FileDeletedTopic);
        services.AddMessageHandler<FilePrimaryStorageChangedEventHandler>(FilePrimaryStorageChangedEventHandler.FilePrimaryStorageChangedTopic);
        services.AddMessageHandler<FileUploadedEventHandler>(FileUploadedEventHandler.FileUploadedTopic);
        services.AddMessageHandler<HierarchyChangedEventHandler>(HierarchyChangedEventHandler.HierarchyChangedTopic);
        services.AddMessageHandler<PermissionChangeLogEventHandler>(PermissionChangeLogEventHandler.AuthorizationChangedTopic);
        services.AddMessageHandler<TenantMembershipChangedEventHandler>(TenantMembershipChangedEventHandler.TenantMembershipChangedTopic);
        services.AddMessageHandler<TenantStatusChangedEventHandler>(TenantStatusChangedEventHandler.TenantStatusChangedTopic);
        services.AddMessageHandler<UserSessionRevokedEventHandler>(UserSessionRevokedEventHandler.UserSessionRevokedTopic);
        services.AddScoped<IAccessLogQueryService, AccessLogQueryService>();
        services.AddScoped<IAccessPolicyHitQueryService, AccessPolicyHitQueryService>();
        services.AddScoped<IAccessPolicyQueryService, AccessPolicyQueryService>();
        services.AddScoped<IApiLogQueryService, ApiLogQueryService>();
        services.AddScoped<IBotConfigQueryService, BotConfigQueryService>();
        services.AddScoped<IConfigQueryService, ConfigQueryService>();
        services.AddScoped<IConstraintRuleQueryService, ConstraintRuleQueryService>();
        services.AddScoped<IDepartmentManagementQueryService, DepartmentManagementQueryService>();
        services.AddScoped<IDepartmentQueryService, DepartmentQueryService>();
        services.AddScoped<IDictQueryService, DictQueryService>();
        services.AddScoped<IDiffLogQueryService, DiffLogQueryService>();
        services.AddScoped<IEmailConfigQueryService, EmailConfigQueryService>();
        services.AddScoped<IExceptionLogQueryService, ExceptionLogQueryService>();
        services.AddScoped<IExportTaskQueryService, ExportTaskQueryService>();
        services.AddScoped<IExternalLoginQueryService, ExternalLoginQueryService>();
        services.AddScoped<IFieldLevelSecurityQueryService, FieldLevelSecurityQueryService>();
        services.AddScoped<IFileQueryService, FileQueryService>();
        services.AddScoped<IImportHistoryQueryService, ImportHistoryQueryService>();
        services.AddScoped<ILoginLogQueryService, LoginLogQueryService>();
        services.AddScoped<IMenuQueryService, MenuQueryService>();
        services.AddScoped<IMessageQueryService, MessageQueryService>();
        services.AddScoped<IMessageTemplateQueryService, MessageTemplateQueryService>();
        services.AddScoped<INotificationQueryService, NotificationQueryService>();
        services.AddScoped<INumberingRuleQueryService, NumberingRuleQueryService>();
        services.AddScoped<IOAuthAppQueryService, OAuthAppQueryService>();
        services.AddScoped<IOAuthCodeQueryService, OAuthCodeQueryService>();
        services.AddScoped<IOAuthTokenQueryService, OAuthTokenQueryService>();
        services.AddScoped<IOnlineUserQueryService, OnlineUserQueryService>();
        services.AddScoped<IOperationLogQueryService, OperationLogQueryService>();
        services.AddScoped<IOperationQueryService, OperationQueryService>();
        services.AddScoped<IPasswordHistoryQueryService, PasswordHistoryQueryService>();
        services.AddScoped<IPermissionCenterQueryService, PermissionCenterQueryService>();
        services.AddScoped<IPermissionChangeLogQueryService, PermissionChangeLogQueryService>();
        services.AddScoped<PermissionConditionInsightQueryService>();
        services.AddScoped<IPermissionConditionQueryService, PermissionConditionQueryService>();
        services.AddScoped<IPermissionDelegationQueryService, PermissionDelegationQueryService>();
        services.AddScoped<IPermissionQueryService, PermissionQueryService>();
        services.AddScoped<IPermissionRequestQueryService, PermissionRequestQueryService>();
        services.AddScoped<IPositionQueryService, PositionQueryService>();
        services.AddScoped<IResourceQueryService, ResourceQueryService>();
        services.AddScoped<IRoleDataScopeQueryService, RoleDataScopeQueryService>();
        services.AddScoped<IRoleHierarchyQueryService, RoleHierarchyQueryService>();
        services.AddScoped<IRoleManagementQueryService, RoleManagementQueryService>();
        services.AddScoped<IRolePermissionQueryService, RolePermissionQueryService>();
        services.AddScoped<IRoleQueryService, RoleQueryService>();
        services.AddScoped<ISessionRoleQueryService, SessionRoleQueryService>();
        services.AddScoped<ISmsConfigQueryService, SmsConfigQueryService>();
        services.AddScoped<IStorageConfigQueryService, StorageConfigQueryService>();
        services.AddScoped<ITaskLogQueryService, TaskLogQueryService>();
        services.AddScoped<ITaskQueryService, TaskQueryService>();
        services.AddScoped<ITelegramBotQueryService, TelegramBotQueryService>();
        services.AddScoped<ITenantEditionPermissionQueryService, TenantEditionPermissionQueryService>();
        services.AddScoped<ITenantEditionQueryService, TenantEditionQueryService>();
        services.AddScoped<ITenantMemberQueryService, TenantMemberQueryService>();
        services.AddScoped<ITenantQueryService, TenantQueryService>();
        services.AddScoped<ITraceQueryService, TraceQueryService>();
        services.AddScoped<IUserDataScopeQueryService, UserDataScopeQueryService>();
        services.AddScoped<IUserDepartmentQueryService, UserDepartmentQueryService>();
        services.AddScoped<IUserManagementQueryService, UserManagementQueryService>();
        services.AddScoped<IUserPermissionQueryService, UserPermissionQueryService>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IUserRoleQueryService, UserRoleQueryService>();
        services.AddScoped<IUserSecurityQueryService, UserSecurityQueryService>();
        services.AddScoped<IUserSessionQueryService, UserSessionQueryService>();
        services.AddScoped<IUserSettingQueryService, UserSettingQueryService>();
        services.AddScoped<IUserStatisticsQueryService, UserStatisticsQueryService>();
        services.AddScoped<IVersionQueryService, VersionQueryService>();
        services.AddScoped<IWorkbenchQueryService, WorkbenchQueryService>();
        services.AddScoped<IUserNotificationDispatchService, UserNotificationDispatchService>();
        services.AddScoped<IUserTaskProgressNotifier, UserTaskProgressNotifier>();
        services.AddScoped<IOpenApiAppService, OpenApiAppService>();
        services.AddScoped<ICurrentTenant, CurrentTenant>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}
