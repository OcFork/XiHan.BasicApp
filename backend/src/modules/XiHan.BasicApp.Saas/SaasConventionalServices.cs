// 由 scratchpad/gen_conventional_services.py 按 4.x 约定生成（仓储基类 / 应用服务基类 → 作用域 + 默认接口）；改动请改脚本重跑。

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XiHan.BasicApp.Saas;

/// <summary>
/// Saas 模块按约定注册的仓储与应用服务（4.x 由框架基类上的依赖标记自动注册；5.0 改为显式登记）
/// </summary>
public static class SaasConventionalServices
{
    /// <summary>
    /// 注册派生自仓储基类与应用服务基类的全部具体类型：类本身 + 默认接口（类名以接口名去 I 结尾），作用域生命周期，幂等
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSaasConventionalServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 仓储
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.AccessPolicyConditionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IAccessPolicyConditionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.AccessPolicyConditionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.AccessPolicyRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IAccessPolicyRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.AccessPolicyRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.BotConfigRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IBotConfigRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.BotConfigRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConfigRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IConfigRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConfigRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConstraintRuleItemRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IConstraintRuleItemRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConstraintRuleItemRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConstraintRuleRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IConstraintRuleRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ConstraintRuleRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.DepartmentHierarchyRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IDepartmentHierarchyRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.DepartmentHierarchyRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.DepartmentRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IDepartmentRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.DepartmentRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.DictItemRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IDictItemRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.DictItemRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.DictRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IDictRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.DictRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.EmailConfigRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IEmailConfigRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.EmailConfigRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.EmailRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IEmailRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.EmailRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ExportTaskRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IExportTaskRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ExportTaskRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ExternalLoginRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IExternalLoginRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ExternalLoginRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.FieldLevelSecurityRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IFieldLevelSecurityRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.FieldLevelSecurityRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.FileRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IFileRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.FileRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.FileStorageRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IFileStorageRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.FileStorageRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ImportHistoryRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IImportHistoryRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ImportHistoryRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.MenuRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IMenuRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.MenuRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.MessageTemplateRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IMessageTemplateRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.MessageTemplateRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.MigrationHistoryRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IMigrationHistoryRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.MigrationHistoryRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.NotificationRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.INotificationRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.NotificationRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.NumberingAllocationRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.INumberingAllocationRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.NumberingAllocationRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.NumberingRuleRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.INumberingRuleRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.NumberingRuleRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthAppRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IOAuthAppRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthAppRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthCodeRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IOAuthCodeRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthCodeRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthTokenRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IOAuthTokenRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.OAuthTokenRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.OperationRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IOperationRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.OperationRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PasswordHistoryRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPasswordHistoryRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PasswordHistoryRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionConditionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPermissionConditionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionConditionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionDelegationRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPermissionDelegationRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionDelegationRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPermissionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionRequestRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPermissionRequestRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PermissionRequestRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.PositionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IPositionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.PositionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.ResourceRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IResourceRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.ResourceRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleDataScopeRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IRoleDataScopeRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleDataScopeRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleHierarchyRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IRoleHierarchyRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleHierarchyRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.RolePermissionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IRolePermissionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.RolePermissionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IRoleRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.RoleRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.SessionRoleRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ISessionRoleRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.SessionRoleRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.SmsConfigRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ISmsConfigRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.SmsConfigRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.SmsRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ISmsRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.SmsRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.StorageConfigRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IStorageConfigRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.StorageConfigRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TaskRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITaskRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TaskRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TelegramBotRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITelegramBotRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TelegramBotRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantEditionPermissionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITenantEditionPermissionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantEditionPermissionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantEditionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITenantEditionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantEditionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITenantRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantUserRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.ITenantUserRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.TenantUserRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserApiCredentialRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserApiCredentialRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserApiCredentialRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserDataScopeRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserDataScopeRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserDataScopeRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserDepartmentRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserDepartmentRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserDepartmentRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserNotificationPreferenceRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserNotificationPreferenceRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserNotificationPreferenceRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserNotificationRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserNotificationRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserNotificationRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserPermissionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserPermissionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserPermissionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserRoleRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserRoleRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserRoleRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSecurityRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserSecurityRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSecurityRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSessionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserSessionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSessionRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSettingRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserSettingRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserSettingRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserStatisticsRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IUserStatisticsRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.UserStatisticsRepository>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Infrastructure.Repositories.VersionRepository>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Domain.Repositories.IVersionRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Infrastructure.Repositories.VersionRepository>());

        // 应用与查询服务
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.AccessPolicyAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IAccessPolicyAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.AccessPolicyAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.AuthAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IAuthAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.AuthAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.BotConfigAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IBotConfigAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.BotConfigAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ConfigAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IConfigAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ConfigAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ConstraintRuleAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IConstraintRuleAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ConstraintRuleAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.DepartmentAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDepartmentAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.DepartmentAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.DictAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDictAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.DictAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.EmailConfigAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IEmailConfigAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.EmailConfigAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ExportTaskAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IExportTaskAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ExportTaskAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.FieldLevelSecurityAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IFieldLevelSecurityAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.FieldLevelSecurityAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.FileAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IFileAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.FileAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ImportHistoryAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IImportHistoryAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ImportHistoryAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.MenuAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMenuAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.MenuAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.MessageAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMessageAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.MessageAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.MessageTemplateAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMessageTemplateAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.MessageTemplateAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.Metadata.EnumMetadataAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.Metadata.TimeZoneMetadataAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.Monitoring.CacheAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.Monitoring.ServerAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.MyOAuthAppAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMyOAuthAppAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.MyOAuthAppAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.NotificationAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.INotificationAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.NotificationAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.NumberingAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.INumberingAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.NumberingAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.NumberingRuleAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.INumberingRuleAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.NumberingRuleAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.OAuthAppAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOAuthAppAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.OAuthAppAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.OAuthConsentAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOAuthConsentAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.OAuthConsentAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.OperationAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOperationAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.OperationAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.PermissionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.PermissionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.PermissionConditionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionConditionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.PermissionConditionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.PermissionDelegationAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionDelegationAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.PermissionDelegationAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.PermissionRequestAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionRequestAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.PermissionRequestAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.PositionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPositionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.PositionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ProfileAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IProfileAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ProfileAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.ResourceAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IResourceAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.ResourceAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.RoleAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRoleAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.RoleAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.SmsConfigAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ISmsConfigAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.SmsConfigAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.StorageConfigAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IStorageConfigAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.StorageConfigAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.TaskAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITaskAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.TaskAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.TelegramBotAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITelegramBotAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.TelegramBotAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.TenantAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.TenantAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.TenantEditionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantEditionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.TenantEditionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserDataScopeAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserDataScopeAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserDataScopeAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserDepartmentAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserDepartmentAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserDepartmentAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserInboxAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserInboxAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserInboxAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserPermissionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserPermissionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserPermissionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserRoleAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserRoleAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserRoleAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserSecurityAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSecurityAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserSecurityAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserSessionAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSessionAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserSessionAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.AppServices.UserSettingAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSettingAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.AppServices.UserSettingAppService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.AccessLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IAccessLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.AccessLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.AccessPolicyHitQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IAccessPolicyHitQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.AccessPolicyHitQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.AccessPolicyQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IAccessPolicyQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.AccessPolicyQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ApiLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IApiLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ApiLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.AuthContextQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IAuthContextQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.AuthContextQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.AuthorizationSnapshotQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IAuthorizationSnapshotQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.AuthorizationSnapshotQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.BotConfigQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IBotConfigQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.BotConfigQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ConfigQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IConfigQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ConfigQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ConstraintRuleQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IConstraintRuleQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ConstraintRuleQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.DepartmentManagementQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDepartmentManagementQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.DepartmentManagementQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.DepartmentQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDepartmentQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.DepartmentQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.DictQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDictQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.DictQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.DiffLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IDiffLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.DiffLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.EmailConfigQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IEmailConfigQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.EmailConfigQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.EnumMetadataQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IEnumMetadataQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.EnumMetadataQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ExceptionLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IExceptionLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ExceptionLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ExportTaskQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IExportTaskQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ExportTaskQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ExternalLoginQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IExternalLoginQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ExternalLoginQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.FieldLevelSecurityQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IFieldLevelSecurityQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.FieldLevelSecurityQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.FileQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IFileQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.FileQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.FileRecordQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IFileRecordQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.FileRecordQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ImportHistoryQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IImportHistoryQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ImportHistoryQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.LoginLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ILoginLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.LoginLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.MenuQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMenuQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.MenuQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.MenuRouteQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IMenuRouteQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.MenuRouteQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.MessageQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMessageQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.MessageQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.MessageRecordQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IMessageRecordQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.MessageRecordQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.MessageTemplateQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IMessageTemplateQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.MessageTemplateQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.NotificationQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.INotificationQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.NotificationQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.NumberingRuleQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.INumberingRuleQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.NumberingRuleQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OAuthAppQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOAuthAppQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OAuthAppQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OAuthCodeQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOAuthCodeQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OAuthCodeQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OAuthTokenQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOAuthTokenQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OAuthTokenQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OnlineUserQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOnlineUserQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OnlineUserQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OperationLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOperationLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OperationLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.OperationQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IOperationQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.OperationQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PasswordHistoryQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPasswordHistoryQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PasswordHistoryQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionCenterQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionCenterQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionCenterQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionChangeLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionChangeLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionChangeLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionConditionInsightQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionConditionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionConditionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionConditionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionDelegationQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionDelegationQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionDelegationQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PermissionRequestQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPermissionRequestQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PermissionRequestQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.PositionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IPositionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.PositionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ProfileQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IProfileQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ProfileQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ResourceQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IResourceQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ResourceQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.RoleDataScopeQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRoleDataScopeQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.RoleDataScopeQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.RoleHierarchyQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRoleHierarchyQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.RoleHierarchyQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.RoleManagementQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRoleManagementQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.RoleManagementQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.RolePermissionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRolePermissionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.RolePermissionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.RoleQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IRoleQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.RoleQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.SaasConfigValueQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ISaasConfigValueQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.SaasConfigValueQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ServerInfoQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IServerInfoQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.ServerInfoQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.SessionRoleQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ISessionRoleQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.SessionRoleQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.SmsConfigQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ISmsConfigQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.SmsConfigQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.StorageConfigQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IStorageConfigQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.StorageConfigQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TaskLogQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITaskLogQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TaskLogQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TaskQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITaskQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TaskQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TaskSchedulerQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.ITaskSchedulerQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TaskSchedulerQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TelegramBotQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITelegramBotQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TelegramBotQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TenantEditionPermissionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantEditionPermissionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TenantEditionPermissionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TenantEditionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantEditionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TenantEditionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TenantMemberQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantMemberQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TenantMemberQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TenantQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITenantQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TenantQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.TraceQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.ITraceQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.TraceQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserDataScopeQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserDataScopeQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserDataScopeQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserDepartmentQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserDepartmentQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserDepartmentQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserInboxQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.IUserInboxQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserInboxQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserManagementQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserManagementQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserManagementQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserPermissionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserPermissionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserPermissionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserRoleQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserRoleQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserRoleQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserSecurityQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSecurityQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserSecurityQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserSessionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSessionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserSessionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserSettingQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserSettingQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserSettingQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.UserStatisticsQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IUserStatisticsQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.UserStatisticsQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.VersionQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IVersionQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.VersionQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.QueryServices.WorkbenchQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.Application.Contracts.IWorkbenchQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.Application.QueryServices.WorkbenchQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Saas.OpenApi.AppServices.OpenApiAppService>();
        services.TryAddScoped<XiHan.BasicApp.Saas.OpenApi.Contracts.IOpenApiAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Saas.OpenApi.AppServices.OpenApiAppService>());

        return services;
    }
}
