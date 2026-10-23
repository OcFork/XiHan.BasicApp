// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application;
using XiHan.BasicApp.Saas.Application.AccessPolicies;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.AppServices.Metadata;
using XiHan.BasicApp.Saas.Application.AppServices.Monitoring;
using XiHan.BasicApp.Saas.Application.Authorization;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Conditions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Dtos.Metadata;
using XiHan.BasicApp.Saas.Application.Dtos.Monitoring;
using XiHan.BasicApp.Saas.Application.Exporting;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Conditions;
using XiHan.BasicApp.Saas.Domain.Configurations;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Identity;
using XiHan.BasicApp.Saas.Domain.Messaging;
using XiHan.BasicApp.Saas.Domain.Numbering;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Domain.Specifications;
using XiHan.BasicApp.Saas.Hubs;
using XiHan.BasicApp.Saas.Infrastructure.Exporting;
using XiHan.BasicApp.Saas.Infrastructure.Messaging;
using XiHan.BasicApp.Saas.OpenApi.AppServices;
using XiHan.BasicApp.Saas.OpenApi.Contracts;
using XiHan.BasicApp.Saas.OpenApi.Dtos;
using XiHan.BasicApp.Saas.Operations.AccessLogQuery;
using XiHan.BasicApp.Saas.Operations.AccessPolicy;
using XiHan.BasicApp.Saas.Operations.AccessPolicyHitQuery;
using XiHan.BasicApp.Saas.Operations.AccessPolicyQuery;
using XiHan.BasicApp.Saas.Operations.ApiLogQuery;
using XiHan.BasicApp.Saas.Operations.Auth;
using XiHan.BasicApp.Saas.Operations.BotConfig;
using XiHan.BasicApp.Saas.Operations.BotConfigQuery;
using XiHan.BasicApp.Saas.Operations.Cache;
using XiHan.BasicApp.Saas.Operations.Config;
using XiHan.BasicApp.Saas.Operations.ConfigQuery;
using XiHan.BasicApp.Saas.Operations.ConstraintRule;
using XiHan.BasicApp.Saas.Operations.ConstraintRuleQuery;
using XiHan.BasicApp.Saas.Operations.Department;
using XiHan.BasicApp.Saas.Operations.DepartmentManagementQuery;
using XiHan.BasicApp.Saas.Operations.DepartmentQuery;
using XiHan.BasicApp.Saas.Operations.Dict;
using XiHan.BasicApp.Saas.Operations.DictQuery;
using XiHan.BasicApp.Saas.Operations.DiffLogQuery;
using XiHan.BasicApp.Saas.Operations.EmailConfig;
using XiHan.BasicApp.Saas.Operations.EmailConfigQuery;
using XiHan.BasicApp.Saas.Operations.EnumMetadata;
using XiHan.BasicApp.Saas.Operations.ExceptionLogQuery;
using XiHan.BasicApp.Saas.Operations.ExportTask;
using XiHan.BasicApp.Saas.Operations.ExportTaskQuery;
using XiHan.BasicApp.Saas.Operations.ExternalLoginQuery;
using XiHan.BasicApp.Saas.Operations.FieldLevelSecurity;
using XiHan.BasicApp.Saas.Operations.FieldLevelSecurityQuery;
using XiHan.BasicApp.Saas.Operations.File;
using XiHan.BasicApp.Saas.Operations.FileQuery;
using XiHan.BasicApp.Saas.Operations.ImportHistory;
using XiHan.BasicApp.Saas.Operations.ImportHistoryQuery;
using XiHan.BasicApp.Saas.Operations.LoginLogQuery;
using XiHan.BasicApp.Saas.Operations.Menu;
using XiHan.BasicApp.Saas.Operations.MenuQuery;
using XiHan.BasicApp.Saas.Operations.Message;
using XiHan.BasicApp.Saas.Operations.MessageQuery;
using XiHan.BasicApp.Saas.Operations.MessageTemplate;
using XiHan.BasicApp.Saas.Operations.MessageTemplateQuery;
using XiHan.BasicApp.Saas.Operations.MyOAuthApp;
using XiHan.BasicApp.Saas.Operations.Notification;
using XiHan.BasicApp.Saas.Operations.NotificationQuery;
using XiHan.BasicApp.Saas.Operations.Numbering;
using XiHan.BasicApp.Saas.Operations.NumberingRule;
using XiHan.BasicApp.Saas.Operations.NumberingRuleQuery;
using XiHan.BasicApp.Saas.Operations.OAuthApp;
using XiHan.BasicApp.Saas.Operations.OAuthAppQuery;
using XiHan.BasicApp.Saas.Operations.OAuthCodeQuery;
using XiHan.BasicApp.Saas.Operations.OAuthConsent;
using XiHan.BasicApp.Saas.Operations.OAuthTokenQuery;
using XiHan.BasicApp.Saas.Operations.OnlineUserQuery;
using XiHan.BasicApp.Saas.Operations.OpenApi;
using XiHan.BasicApp.Saas.Operations.Operation;
using XiHan.BasicApp.Saas.Operations.OperationLogQuery;
using XiHan.BasicApp.Saas.Operations.OperationQuery;
using XiHan.BasicApp.Saas.Operations.PasswordHistoryQuery;
using XiHan.BasicApp.Saas.Operations.Permission;
using XiHan.BasicApp.Saas.Operations.PermissionCenterQuery;
using XiHan.BasicApp.Saas.Operations.PermissionChangeLogQuery;
using XiHan.BasicApp.Saas.Operations.PermissionCondition;
using XiHan.BasicApp.Saas.Operations.PermissionConditionInsightQuery;
using XiHan.BasicApp.Saas.Operations.PermissionConditionQuery;
using XiHan.BasicApp.Saas.Operations.PermissionDelegation;
using XiHan.BasicApp.Saas.Operations.PermissionDelegationQuery;
using XiHan.BasicApp.Saas.Operations.PermissionQuery;
using XiHan.BasicApp.Saas.Operations.PermissionRequest;
using XiHan.BasicApp.Saas.Operations.PermissionRequestQuery;
using XiHan.BasicApp.Saas.Operations.Position;
using XiHan.BasicApp.Saas.Operations.PositionQuery;
using XiHan.BasicApp.Saas.Operations.Profile;
using XiHan.BasicApp.Saas.Operations.Resource;
using XiHan.BasicApp.Saas.Operations.ResourceQuery;
using XiHan.BasicApp.Saas.Operations.Role;
using XiHan.BasicApp.Saas.Operations.RoleDataScopeQuery;
using XiHan.BasicApp.Saas.Operations.RoleHierarchyQuery;
using XiHan.BasicApp.Saas.Operations.RoleManagementQuery;
using XiHan.BasicApp.Saas.Operations.RolePermissionQuery;
using XiHan.BasicApp.Saas.Operations.RoleQuery;
using XiHan.BasicApp.Saas.Operations.Server;
using XiHan.BasicApp.Saas.Operations.SessionRoleQuery;
using XiHan.BasicApp.Saas.Operations.SmsConfig;
using XiHan.BasicApp.Saas.Operations.SmsConfigQuery;
using XiHan.BasicApp.Saas.Operations.StorageConfig;
using XiHan.BasicApp.Saas.Operations.StorageConfigQuery;
using XiHan.BasicApp.Saas.Operations.Task;
using XiHan.BasicApp.Saas.Operations.TaskLogQuery;
using XiHan.BasicApp.Saas.Operations.TaskQuery;
using XiHan.BasicApp.Saas.Operations.TelegramBot;
using XiHan.BasicApp.Saas.Operations.TelegramBotQuery;
using XiHan.BasicApp.Saas.Operations.Tenant;
using XiHan.BasicApp.Saas.Operations.TenantEdition;
using XiHan.BasicApp.Saas.Operations.TenantEditionPermissionQuery;
using XiHan.BasicApp.Saas.Operations.TenantEditionQuery;
using XiHan.BasicApp.Saas.Operations.TenantMemberQuery;
using XiHan.BasicApp.Saas.Operations.TenantQuery;
using XiHan.BasicApp.Saas.Operations.TimeZoneMetadata;
using XiHan.BasicApp.Saas.Operations.TraceQuery;
using XiHan.BasicApp.Saas.Operations.User;
using XiHan.BasicApp.Saas.Operations.UserDataScope;
using XiHan.BasicApp.Saas.Operations.UserDataScopeQuery;
using XiHan.BasicApp.Saas.Operations.UserDepartment;
using XiHan.BasicApp.Saas.Operations.UserDepartmentQuery;
using XiHan.BasicApp.Saas.Operations.UserInbox;
using XiHan.BasicApp.Saas.Operations.UserManagementQuery;
using XiHan.BasicApp.Saas.Operations.UserPermission;
using XiHan.BasicApp.Saas.Operations.UserPermissionQuery;
using XiHan.BasicApp.Saas.Operations.UserQuery;
using XiHan.BasicApp.Saas.Operations.UserRole;
using XiHan.BasicApp.Saas.Operations.UserRoleQuery;
using XiHan.BasicApp.Saas.Operations.UserSecurity;
using XiHan.BasicApp.Saas.Operations.UserSecurityQuery;
using XiHan.BasicApp.Saas.Operations.UserSession;
using XiHan.BasicApp.Saas.Operations.UserSessionQuery;
using XiHan.BasicApp.Saas.Operations.UserSetting;
using XiHan.BasicApp.Saas.Operations.UserSettingQuery;
using XiHan.BasicApp.Saas.Operations.UserStatisticsQuery;
using XiHan.BasicApp.Saas.Operations.VersionQuery;
using XiHan.BasicApp.Core.Entities;
using XiHan.BasicApp.Core.Repositories;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Localization.Abstractions;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Utils.Diagnostics.HardwareInfos;
using XiHan.Framework.Utils.Reflections;
using XiHan.Framework.Utils.Runtime;

namespace XiHan.BasicApp.Saas.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(AccessLogDetailDto))]
[JsonSerializable(typeof(AccessLogPageQueryDto))]
[JsonSerializable(typeof(AccessPolicyActionUpdateDto))]
[JsonSerializable(typeof(AccessPolicyCreateDto))]
[JsonSerializable(typeof(AccessPolicyDetailDto))]
[JsonSerializable(typeof(AccessPolicyHitListItemDto))]
[JsonSerializable(typeof(AccessPolicyHitPageQueryDto))]
[JsonSerializable(typeof(AccessPolicyPageQueryDto))]
[JsonSerializable(typeof(AccessPolicyStatusUpdateDto))]
[JsonSerializable(typeof(AccessPolicyTrialInputDto))]
[JsonSerializable(typeof(AccessPolicyTrialResultDto))]
[JsonSerializable(typeof(AccessPolicyUpdateDto))]
[JsonSerializable(typeof(ApiLogDetailDto))]
[JsonSerializable(typeof(ApiLogPageQueryDto))]
[JsonSerializable(typeof(BoardInfo))]
[JsonSerializable(typeof(BotConfigCreateDto))]
[JsonSerializable(typeof(BotConfigDefaultUpdateDto))]
[JsonSerializable(typeof(BotConfigDetailDto))]
[JsonSerializable(typeof(BotConfigPageQueryDto))]
[JsonSerializable(typeof(BotConfigStatusUpdateDto))]
[JsonSerializable(typeof(BotConfigUpdateDto))]
[JsonSerializable(typeof(CacheStringUpdateDto))]
[JsonSerializable(typeof(CancelRequest))]
[JsonSerializable(typeof(CaptchaChallengeDto))]
[JsonSerializable(typeof(ConfigCreateDto))]
[JsonSerializable(typeof(ConfigDetailDto))]
[JsonSerializable(typeof(ConfigPageQueryDto))]
[JsonSerializable(typeof(ConfigStatusUpdateDto))]
[JsonSerializable(typeof(ConfigUpdateDto))]
[JsonSerializable(typeof(ConstraintRuleCreateDto))]
[JsonSerializable(typeof(ConstraintRuleDetailDto))]
[JsonSerializable(typeof(ConstraintRulePageQueryDto))]
[JsonSerializable(typeof(ConstraintRuleStatusUpdateDto))]
[JsonSerializable(typeof(ConstraintRuleUpdateDto))]
[JsonSerializable(typeof(CpuInfo))]
[JsonSerializable(typeof(CreateOAuthBindTicketRequest))]
[JsonSerializable(typeof(DeleteAccessPolicyRequest))]
[JsonSerializable(typeof(DeleteApiCredentialRequest))]
[JsonSerializable(typeof(DeleteBotConfigRequest))]
[JsonSerializable(typeof(DeleteConfigRequest))]
[JsonSerializable(typeof(DeleteConstraintRuleRequest))]
[JsonSerializable(typeof(DeleteDepartmentRequest))]
[JsonSerializable(typeof(DeleteDictItemRequest))]
[JsonSerializable(typeof(DeleteDictRequest))]
[JsonSerializable(typeof(DeleteEmailConfigRequest))]
[JsonSerializable(typeof(DeleteEmailRequest))]
[JsonSerializable(typeof(DeleteFieldLevelSecurityRequest))]
[JsonSerializable(typeof(DeleteMenuRequest))]
[JsonSerializable(typeof(DeleteMessageTemplateRequest))]
[JsonSerializable(typeof(DeleteMyOAuthAppRequest))]
[JsonSerializable(typeof(DeleteNotificationRequest))]
[JsonSerializable(typeof(DeleteNumberingRuleRequest))]
[JsonSerializable(typeof(DeleteOAuthAppRequest))]
[JsonSerializable(typeof(DeleteOperationRequest))]
[JsonSerializable(typeof(DeletePermissionConditionRequest))]
[JsonSerializable(typeof(DeletePermissionDelegationRequest))]
[JsonSerializable(typeof(DeletePermissionRequest))]
[JsonSerializable(typeof(DeletePermissionRequestRequest))]
[JsonSerializable(typeof(DeletePositionRequest))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(DeleteResourceRequest))]
[JsonSerializable(typeof(DeleteRoleRequest))]
[JsonSerializable(typeof(DeleteSmsConfigRequest))]
[JsonSerializable(typeof(DeleteSmsRequest))]
[JsonSerializable(typeof(DeleteStorageConfigRequest))]
[JsonSerializable(typeof(DeleteTaskRequest))]
[JsonSerializable(typeof(DeleteTelegramBotRequest))]
[JsonSerializable(typeof(DeleteTenantMemberRequest))]
[JsonSerializable(typeof(DeleteTenantRequest))]
[JsonSerializable(typeof(DeleteUserRequest))]
[JsonSerializable(typeof(DepartmentCreateDto))]
[JsonSerializable(typeof(DepartmentDetailDto))]
[JsonSerializable(typeof(DepartmentManagementDetailDto))]
[JsonSerializable(typeof(DepartmentPageQueryDto))]
[JsonSerializable(typeof(DepartmentStatusUpdateDto))]
[JsonSerializable(typeof(DepartmentTreeQueryDto))]
[JsonSerializable(typeof(DepartmentUpdateDto))]
[JsonSerializable(typeof(DictCreateDto))]
[JsonSerializable(typeof(DictDetailDto))]
[JsonSerializable(typeof(DictItemCreateDto))]
[JsonSerializable(typeof(DictItemDetailDto))]
[JsonSerializable(typeof(DictItemPageQueryDto))]
[JsonSerializable(typeof(DictItemStatusUpdateDto))]
[JsonSerializable(typeof(DictItemTreeQueryDto))]
[JsonSerializable(typeof(DictItemUpdateDto))]
[JsonSerializable(typeof(DictPageQueryDto))]
[JsonSerializable(typeof(DictStatusUpdateDto))]
[JsonSerializable(typeof(DictUpdateDto))]
[JsonSerializable(typeof(DiffLogDetailDto))]
[JsonSerializable(typeof(DiffLogPageQueryDto))]
[JsonSerializable(typeof(EmailConfigCreateDto))]
[JsonSerializable(typeof(EmailConfigDefaultUpdateDto))]
[JsonSerializable(typeof(EmailConfigDetailDto))]
[JsonSerializable(typeof(EmailConfigPageQueryDto))]
[JsonSerializable(typeof(EmailConfigStatusUpdateDto))]
[JsonSerializable(typeof(EmailConfigUpdateDto))]
[JsonSerializable(typeof(EmailCreateDto))]
[JsonSerializable(typeof(EmailDetailDto))]
[JsonSerializable(typeof(EmailLoginCodeRequestDto))]
[JsonSerializable(typeof(EmailLoginRequestDto))]
[JsonSerializable(typeof(EmailPageQueryDto))]
[JsonSerializable(typeof(EmailStatusUpdateDto))]
[JsonSerializable(typeof(EmailUpdateDto))]
[JsonSerializable(typeof(EmptyResponse))]
[JsonSerializable(typeof(EnumMetadataDto))]
[JsonSerializable(typeof(ExceptionLogDetailDto))]
[JsonSerializable(typeof(ExceptionLogPageQueryDto))]
[JsonSerializable(typeof(ExistsRequest))]
[JsonSerializable(typeof(ExportTaskDto))]
[JsonSerializable(typeof(ExportTaskPageQueryDto))]
[JsonSerializable(typeof(ExportTaskSubmitDto))]
[JsonSerializable(typeof(ExternalLoginDetailDto))]
[JsonSerializable(typeof(ExternalLoginPageQueryDto))]
[JsonSerializable(typeof(FieldLevelSecurityCreateDto))]
[JsonSerializable(typeof(FieldLevelSecurityDetailDto))]
[JsonSerializable(typeof(FieldLevelSecurityPageQueryDto))]
[JsonSerializable(typeof(FieldLevelSecurityStatusUpdateDto))]
[JsonSerializable(typeof(FieldLevelSecurityUpdateDto))]
[JsonSerializable(typeof(FileDeleteDto))]
[JsonSerializable(typeof(FileDetailDto))]
[JsonSerializable(typeof(FileFastUploadDto))]
[JsonSerializable(typeof(FileMetadataUpdateDto))]
[JsonSerializable(typeof(FilePageQueryDto))]
[JsonSerializable(typeof(FilePrimaryStorageSwitchDto))]
[JsonSerializable(typeof(FileStatusUpdateDto))]
[JsonSerializable(typeof(FileStorageDetailDto))]
[JsonSerializable(typeof(FileStoragePageQueryDto))]
[JsonSerializable(typeof(FileStorageStatusUpdateDto))]
[JsonSerializable(typeof(FileStorageVerifyDto))]
[JsonSerializable(typeof(FileUploadDto))]
[JsonSerializable(typeof(GenerateFilePresignedUrlRequest))]
[JsonSerializable(typeof(GetAccessLogDetailRequest))]
[JsonSerializable(typeof(GetAccessPolicyAttributesRequest))]
[JsonSerializable(typeof(GetAccessPolicyDetailRequest))]
[JsonSerializable(typeof(GetAccessPolicyHitDetailRequest))]
[JsonSerializable(typeof(GetAccessPolicyTemplatesRequest))]
[JsonSerializable(typeof(GetActivityRequest))]
[JsonSerializable(typeof(GetAllEnumsRequest))]
[JsonSerializable(typeof(GetApiCredentialsRequest))]
[JsonSerializable(typeof(GetApiLogDetailRequest))]
[JsonSerializable(typeof(GetBannerRequest))]
[JsonSerializable(typeof(GetBoardInfoRequest))]
[JsonSerializable(typeof(GetBotConfigDetailRequest))]
[JsonSerializable(typeof(GetCaptchaRequest))]
[JsonSerializable(typeof(GetConditionAttributesRequest))]
[JsonSerializable(typeof(GetConfigDetailRequest))]
[JsonSerializable(typeof(GetConstraintRuleDetailRequest))]
[JsonSerializable(typeof(GetCpuInfoRequest))]
[JsonSerializable(typeof(GetDepartmentDetailRequest))]
[JsonSerializable(typeof(GetDepartmentManagementDetailRequest))]
[JsonSerializable(typeof(GetDepartmentUsersRequest))]
[JsonSerializable(typeof(GetDetailRequest))]
[JsonSerializable(typeof(GetDictDetailRequest))]
[JsonSerializable(typeof(GetDictItemDetailRequest))]
[JsonSerializable(typeof(GetDictOptionsRequest))]
[JsonSerializable(typeof(GetDiffLogDetailRequest))]
[JsonSerializable(typeof(GetDiskInfoRequest))]
[JsonSerializable(typeof(GetEmailConfigDetailRequest))]
[JsonSerializable(typeof(GetEmailDetailRequest))]
[JsonSerializable(typeof(GetEnabledDepartmentsRequest))]
[JsonSerializable(typeof(GetEnabledPositionsRequest))]
[JsonSerializable(typeof(GetEnabledTenantEditionsRequest))]
[JsonSerializable(typeof(GetEnumRequest))]
[JsonSerializable(typeof(GetExceptionLogDetailRequest))]
[JsonSerializable(typeof(GetExternalLoginDetailRequest))]
[JsonSerializable(typeof(GetFieldLevelSecurityDetailRequest))]
[JsonSerializable(typeof(GetFieldSecurityEntitiesRequest))]
[JsonSerializable(typeof(GetFileDetailRequest))]
[JsonSerializable(typeof(GetFileStorageDetailRequest))]
[JsonSerializable(typeof(GetFileStoragesRequest))]
[JsonSerializable(typeof(GetGpuInfoRequest))]
[JsonSerializable(typeof(GetKeysRequest))]
[JsonSerializable(typeof(GetLinkedAccountsRequest))]
[JsonSerializable(typeof(GetListRequest))]
[JsonSerializable(typeof(GetLoginBansRequest))]
[JsonSerializable(typeof(GetLoginConfigRequest))]
[JsonSerializable(typeof(GetLoginLogDetailRequest))]
[JsonSerializable(typeof(GetLoginLogsRequest))]
[JsonSerializable(typeof(GetMandatoryUnreadRequest))]
[JsonSerializable(typeof(GetMemoryInfoRequest))]
[JsonSerializable(typeof(GetMenuDetailRequest))]
[JsonSerializable(typeof(GetMessageTemplateDetailRequest))]
[JsonSerializable(typeof(GetMigrationHistoryDetailRequest))]
[JsonSerializable(typeof(GetMineRequest))]
[JsonSerializable(typeof(GetMyAvailableTenantsRequest))]
[JsonSerializable(typeof(GetMyOAuthAppsRequest))]
[JsonSerializable(typeof(GetMySubscriptionRequest))]
[JsonSerializable(typeof(GetNetworkInfoRequest))]
[JsonSerializable(typeof(GetNotificationDetailRequest))]
[JsonSerializable(typeof(GetNotificationPreferenceRequest))]
[JsonSerializable(typeof(GetNotificationReadStatsRequest))]
[JsonSerializable(typeof(GetNuGetPackagesRequest))]
[JsonSerializable(typeof(GetNumberingRuleDetailRequest))]
[JsonSerializable(typeof(GetOAuthAppDetailRequest))]
[JsonSerializable(typeof(GetOAuthCodeDetailRequest))]
[JsonSerializable(typeof(GetOAuthTokenDetailRequest))]
[JsonSerializable(typeof(GetOnlineUserSummaryRequest))]
[JsonSerializable(typeof(GetOperationDetailRequest))]
[JsonSerializable(typeof(GetOperationLogDetailRequest))]
[JsonSerializable(typeof(GetOverQuotaTenantsRequest))]
[JsonSerializable(typeof(GetPasswordHistoryDetailRequest))]
[JsonSerializable(typeof(GetPermissionCatalogRequest))]
[JsonSerializable(typeof(GetPermissionCenterDetailRequest))]
[JsonSerializable(typeof(GetPermissionChangeLogDetailRequest))]
[JsonSerializable(typeof(GetPermissionConditionDetailRequest))]
[JsonSerializable(typeof(GetPermissionConditionInventoryRequest))]
[JsonSerializable(typeof(GetPermissionDelegationDetailRequest))]
[JsonSerializable(typeof(GetPermissionDetailRequest))]
[JsonSerializable(typeof(GetPermissionRequestDetailRequest))]
[JsonSerializable(typeof(GetPermissionsRequest))]
[JsonSerializable(typeof(GetPopupRequest))]
[JsonSerializable(typeof(GetPositionDetailRequest))]
[JsonSerializable(typeof(GetProfileRequest))]
[JsonSerializable(typeof(GetRequest))]
[JsonSerializable(typeof(GetResourceDetailRequest))]
[JsonSerializable(typeof(GetRoleAncestorsRequest))]
[JsonSerializable(typeof(GetRoleDataScopeDetailRequest))]
[JsonSerializable(typeof(GetRoleDataScopesRequest))]
[JsonSerializable(typeof(GetRoleDescendantsRequest))]
[JsonSerializable(typeof(GetRoleDetailRequest))]
[JsonSerializable(typeof(GetRoleInheritedPermissionsRequest))]
[JsonSerializable(typeof(GetRoleManagementDetailRequest))]
[JsonSerializable(typeof(GetRoleMembersRequest))]
[JsonSerializable(typeof(GetRolePermissionConditionsRequest))]
[JsonSerializable(typeof(GetRolePermissionDetailRequest))]
[JsonSerializable(typeof(GetRolePermissionsRequest))]
[JsonSerializable(typeof(GetRuntimeInfoRequest))]
[JsonSerializable(typeof(GetServerInfoRequest))]
[JsonSerializable(typeof(GetSessionRoleDetailRequest))]
[JsonSerializable(typeof(GetSessionsRequest))]
[JsonSerializable(typeof(GetSmsConfigDetailRequest))]
[JsonSerializable(typeof(GetSmsDetailRequest))]
[JsonSerializable(typeof(GetStorageConfigDetailRequest))]
[JsonSerializable(typeof(GetStringRequest))]
[JsonSerializable(typeof(GetTaskDetailRequest))]
[JsonSerializable(typeof(GetTaskLogDetailRequest))]
[JsonSerializable(typeof(GetTelegramBotDetailRequest))]
[JsonSerializable(typeof(GetTenantDetailRequest))]
[JsonSerializable(typeof(GetTenantEditionDetailRequest))]
[JsonSerializable(typeof(GetTenantEditionPermissionDetailRequest))]
[JsonSerializable(typeof(GetTenantEditionPermissionsRequest))]
[JsonSerializable(typeof(GetTenantMemberDetailRequest))]
[JsonSerializable(typeof(GetTimeZoneOptionsRequest))]
[JsonSerializable(typeof(GetUserDataScopeDetailRequest))]
[JsonSerializable(typeof(GetUserDataScopeSettingRequest))]
[JsonSerializable(typeof(GetUserDataScopesRequest))]
[JsonSerializable(typeof(GetUserDepartmentDetailRequest))]
[JsonSerializable(typeof(GetUserDepartmentsRequest))]
[JsonSerializable(typeof(GetUserDetailRequest))]
[JsonSerializable(typeof(GetUserInfoRequest))]
[JsonSerializable(typeof(GetUserManagementDetailRequest))]
[JsonSerializable(typeof(GetUserNotificationDetailRequest))]
[JsonSerializable(typeof(GetUserPermissionConditionsRequest))]
[JsonSerializable(typeof(GetUserPermissionDetailRequest))]
[JsonSerializable(typeof(GetUserPermissionsRequest))]
[JsonSerializable(typeof(GetUserRoleDetailRequest))]
[JsonSerializable(typeof(GetUserRolesRequest))]
[JsonSerializable(typeof(GetUserSecurityDetailRequest))]
[JsonSerializable(typeof(GetUserSessionDetailRequest))]
[JsonSerializable(typeof(GetUserStatisticsDetailRequest))]
[JsonSerializable(typeof(GetVersionDetailRequest))]
[JsonSerializable(typeof(IReadOnlyCollection<string>))]
[JsonSerializable(typeof(IReadOnlyList<DepartmentListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<DepartmentTreeNodeDto>))]
[JsonSerializable(typeof(IReadOnlyList<DictItemTreeNodeDto>))]
[JsonSerializable(typeof(IReadOnlyList<DictOptionDto>))]
[JsonSerializable(typeof(IReadOnlyList<FieldSecurityEntityDto>))]
[JsonSerializable(typeof(IReadOnlyList<FileStorageListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<MenuListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<MenuTreeNodeDto>))]
[JsonSerializable(typeof(IReadOnlyList<OperationSelectItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<PermissionConditionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<PermissionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<PermissionSelectItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<PositionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<ResourceSelectItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<RoleDataScopeListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<RoleInheritanceItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<RoleInheritedPermissionDto>))]
[JsonSerializable(typeof(IReadOnlyList<RoleMemberDto>))]
[JsonSerializable(typeof(IReadOnlyList<RolePermissionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<RoleSelectItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<TenantEditionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<TenantEditionPermissionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<TenantOverQuotaDto>))]
[JsonSerializable(typeof(IReadOnlyList<TenantSwitcherDto>))]
[JsonSerializable(typeof(IReadOnlyList<TimeZoneOptionDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserDataScopeListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserDepartmentListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserPermissionListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserRoleListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserSelectItemDto>))]
[JsonSerializable(typeof(ImportHistoryCreateDto))]
[JsonSerializable(typeof(ImportHistoryDto))]
[JsonSerializable(typeof(InitializeDatabaseRequest))]
[JsonSerializable(typeof(List<AccessPolicyAttributeDto>))]
[JsonSerializable(typeof(List<AccessPolicyTemplateDto>))]
[JsonSerializable(typeof(List<DiskInfo>))]
[JsonSerializable(typeof(List<EnumMetadataDto>))]
[JsonSerializable(typeof(List<GpuInfo>))]
[JsonSerializable(typeof(List<ImportHistoryDto>))]
[JsonSerializable(typeof(List<LoginBanDto>))]
[JsonSerializable(typeof(List<MyOAuthAppItemDto>))]
[JsonSerializable(typeof(List<NetworkInfo>))]
[JsonSerializable(typeof(List<NuGetPackage>))]
[JsonSerializable(typeof(List<PermissionConditionAttributeDto>))]
[JsonSerializable(typeof(List<ProfileApiCredentialDto>))]
[JsonSerializable(typeof(List<ProfileExternalLoginDto>))]
[JsonSerializable(typeof(List<ProfileSessionDto>))]
[JsonSerializable(typeof(List<UserInboxItemDto>))]
[JsonSerializable(typeof(LockSessionRequestDto))]
[JsonSerializable(typeof(LoginConfigDto))]
[JsonSerializable(typeof(LoginLogDetailDto))]
[JsonSerializable(typeof(LoginLogPageQueryDto))]
[JsonSerializable(typeof(LoginRequestDto))]
[JsonSerializable(typeof(LoginResponseDto))]
[JsonSerializable(typeof(LoginTokenDto))]
[JsonSerializable(typeof(LoginUnbanDto))]
[JsonSerializable(typeof(LogoutRequest))]
[JsonSerializable(typeof(MarkAllReadRequest))]
[JsonSerializable(typeof(MenuCreateDto))]
[JsonSerializable(typeof(MenuDetailDto))]
[JsonSerializable(typeof(MenuListQueryDto))]
[JsonSerializable(typeof(MenuPageQueryDto))]
[JsonSerializable(typeof(MenuStatusUpdateDto))]
[JsonSerializable(typeof(MenuTreeQueryDto))]
[JsonSerializable(typeof(MenuUpdateDto))]
[JsonSerializable(typeof(MessageTemplateCreateDto))]
[JsonSerializable(typeof(MessageTemplateDetailDto))]
[JsonSerializable(typeof(MessageTemplatePageQueryDto))]
[JsonSerializable(typeof(MessageTemplateStatusUpdateDto))]
[JsonSerializable(typeof(MessageTemplateUpdateDto))]
[JsonSerializable(typeof(MigrationHistoryDetailDto))]
[JsonSerializable(typeof(MigrationHistoryPageQueryDto))]
[JsonSerializable(typeof(MyOAuthAppCreateDto))]
[JsonSerializable(typeof(MyOAuthAppItemDto))]
[JsonSerializable(typeof(MyOAuthAppSecretDto))]
[JsonSerializable(typeof(MyOAuthAppStatusDto))]
[JsonSerializable(typeof(MyOAuthAppUpdateDto))]
[JsonSerializable(typeof(NotificationCreateDto))]
[JsonSerializable(typeof(NotificationDetailDto))]
[JsonSerializable(typeof(NotificationPageQueryDto))]
[JsonSerializable(typeof(NotificationPublishDto))]
[JsonSerializable(typeof(NotificationPublishResultDto))]
[JsonSerializable(typeof(NotificationReadStatsDto))]
[JsonSerializable(typeof(NotificationUnreadUserPageQueryDto))]
[JsonSerializable(typeof(NotificationUpdateDto))]
[JsonSerializable(typeof(NumberBatchGenerateDto))]
[JsonSerializable(typeof(NumberGenerateDto))]
[JsonSerializable(typeof(NumberGenerationResultDto))]
[JsonSerializable(typeof(NumberingAllocationPageQueryDto))]
[JsonSerializable(typeof(NumberingBatchPreviewDto))]
[JsonSerializable(typeof(NumberingBatchPreviewResultDto))]
[JsonSerializable(typeof(NumberingPreviewDto))]
[JsonSerializable(typeof(NumberingPreviewResultDto))]
[JsonSerializable(typeof(NumberingRuleCreateDto))]
[JsonSerializable(typeof(NumberingRuleDetailDto))]
[JsonSerializable(typeof(NumberingRulePageQueryDto))]
[JsonSerializable(typeof(NumberingRuleResetDto))]
[JsonSerializable(typeof(NumberingRuleStatusUpdateDto))]
[JsonSerializable(typeof(NumberingRuleUpdateDto))]
[JsonSerializable(typeof(OAuthAppCreateDto))]
[JsonSerializable(typeof(OAuthAppDetailDto))]
[JsonSerializable(typeof(OAuthAppPageQueryDto))]
[JsonSerializable(typeof(OAuthAppSecretDto))]
[JsonSerializable(typeof(OAuthAppStatusUpdateDto))]
[JsonSerializable(typeof(OAuthAppUpdateDto))]
[JsonSerializable(typeof(OAuthAuthorizeRequestDto))]
[JsonSerializable(typeof(OAuthCodeDetailDto))]
[JsonSerializable(typeof(OAuthCodePageQueryDto))]
[JsonSerializable(typeof(OAuthConsentPreviewDto))]
[JsonSerializable(typeof(OAuthConsentResultDto))]
[JsonSerializable(typeof(OAuthTokenDetailDto))]
[JsonSerializable(typeof(OAuthTokenPageQueryDto))]
[JsonSerializable(typeof(OnlineUserPageQueryDto))]
[JsonSerializable(typeof(OnlineUserSummaryDto))]
[JsonSerializable(typeof(OpenApiEchoDto))]
[JsonSerializable(typeof(OpenApiEchoInputDto))]
[JsonSerializable(typeof(OpenApiPingDto))]
[JsonSerializable(typeof(OperationCreateDto))]
[JsonSerializable(typeof(OperationDetailDto))]
[JsonSerializable(typeof(OperationLogDetailDto))]
[JsonSerializable(typeof(OperationLogPageQueryDto))]
[JsonSerializable(typeof(OperationPageQueryDto))]
[JsonSerializable(typeof(OperationSelectQueryDto))]
[JsonSerializable(typeof(OperationStatusUpdateDto))]
[JsonSerializable(typeof(OperationUpdateDto))]
[JsonSerializable(typeof(PageResultDtoBase<AccessLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<AccessPolicyHitListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<AccessPolicyListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ApiLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<BotConfigListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ConfigListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ConstraintRuleListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<DepartmentListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<DictItemListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<DictListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<DiffLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<EmailConfigListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<EmailListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ExceptionLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ExportTaskDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ExternalLoginListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<FieldLevelSecurityListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<FileListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<FileStorageListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<LoginLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<MenuListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<MessageTemplateListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<MigrationHistoryListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<NotificationListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<NotificationUnreadUserDto>))]
[JsonSerializable(typeof(PageResultDtoBase<NumberingAllocationListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<NumberingRuleListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OAuthAppListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OAuthCodeListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OAuthTokenListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OnlineUserListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OperationListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<OperationLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PasswordHistoryListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PermissionChangeLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PermissionDelegationListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PermissionListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PermissionRequestListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PositionListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ResourceListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<RoleListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<SessionRoleListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<SmsConfigListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<SmsListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<StorageConfigListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TaskListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TaskLogListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TelegramBotListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TenantEditionListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TenantListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<TenantMemberListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<UserListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<UserNotificationListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<UserSecurityListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<UserSessionListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<UserStatisticsListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<VersionListItemDto>))]
[JsonSerializable(typeof(PasswordHistoryDetailDto))]
[JsonSerializable(typeof(PasswordHistoryPageQueryDto))]
[JsonSerializable(typeof(PasswordResetConfirmDto))]
[JsonSerializable(typeof(PasswordResetConfirmResultDto))]
[JsonSerializable(typeof(PasswordResetRequestDto))]
[JsonSerializable(typeof(PasswordResetResultDto))]
[JsonSerializable(typeof(PermissionCenterDetailDto))]
[JsonSerializable(typeof(PermissionChangeLogDetailDto))]
[JsonSerializable(typeof(PermissionChangeLogPageQueryDto))]
[JsonSerializable(typeof(PermissionConditionBatchTrialInputDto))]
[JsonSerializable(typeof(PermissionConditionBatchTrialResultDto))]
[JsonSerializable(typeof(PermissionConditionCreateDto))]
[JsonSerializable(typeof(PermissionConditionDetailDto))]
[JsonSerializable(typeof(PermissionConditionInventoryDto))]
[JsonSerializable(typeof(PermissionConditionStatusUpdateDto))]
[JsonSerializable(typeof(PermissionConditionTrialInputDto))]
[JsonSerializable(typeof(PermissionConditionTrialResultDto))]
[JsonSerializable(typeof(PermissionConditionUpdateDto))]
[JsonSerializable(typeof(PermissionCreateDto))]
[JsonSerializable(typeof(PermissionDelegationCreateDto))]
[JsonSerializable(typeof(PermissionDelegationDetailDto))]
[JsonSerializable(typeof(PermissionDelegationPageQueryDto))]
[JsonSerializable(typeof(PermissionDelegationStatusUpdateDto))]
[JsonSerializable(typeof(PermissionDelegationUpdateDto))]
[JsonSerializable(typeof(PermissionDetailDto))]
[JsonSerializable(typeof(PermissionInfoDto))]
[JsonSerializable(typeof(PermissionPageQueryDto))]
[JsonSerializable(typeof(PermissionRequestCreateDto))]
[JsonSerializable(typeof(PermissionRequestDetailDto))]
[JsonSerializable(typeof(PermissionRequestPageQueryDto))]
[JsonSerializable(typeof(PermissionRequestStatusUpdateDto))]
[JsonSerializable(typeof(PermissionRequestUpdateDto))]
[JsonSerializable(typeof(PermissionSelectQueryDto))]
[JsonSerializable(typeof(PermissionStatusUpdateDto))]
[JsonSerializable(typeof(PermissionUpdateDto))]
[JsonSerializable(typeof(PhoneLoginCodeRequestDto))]
[JsonSerializable(typeof(PhoneLoginRequestDto))]
[JsonSerializable(typeof(PingRequest))]
[JsonSerializable(typeof(PositionCreateDto))]
[JsonSerializable(typeof(PositionDetailDto))]
[JsonSerializable(typeof(PositionPageQueryDto))]
[JsonSerializable(typeof(PositionStatusUpdateDto))]
[JsonSerializable(typeof(PositionUpdateDto))]
[JsonSerializable(typeof(ProfileActivityDto))]
[JsonSerializable(typeof(ProfileApiCredentialCreateDto))]
[JsonSerializable(typeof(ProfileApiCredentialDto))]
[JsonSerializable(typeof(ProfileApiCredentialIdDto))]
[JsonSerializable(typeof(ProfileApiCredentialSecretDto))]
[JsonSerializable(typeof(ProfileApiCredentialStatusDto))]
[JsonSerializable(typeof(ProfileChangeEmailDto))]
[JsonSerializable(typeof(ProfileChangePasswordDto))]
[JsonSerializable(typeof(ProfileChangePhoneDto))]
[JsonSerializable(typeof(ProfileChangeUserNameDto))]
[JsonSerializable(typeof(ProfileLoginLogPageDto))]
[JsonSerializable(typeof(ProfileNotificationPreferenceDto))]
[JsonSerializable(typeof(ProfilePasswordConfirmDto))]
[JsonSerializable(typeof(ProfileSessionRevokeDto))]
[JsonSerializable(typeof(ProfileTwoFactorMethodDto))]
[JsonSerializable(typeof(ProfileTwoFactorSetupDto))]
[JsonSerializable(typeof(ProfileTwoFactorVerifyDto))]
[JsonSerializable(typeof(ProfileUnlinkAccountDto))]
[JsonSerializable(typeof(ProfileUpdateDto))]
[JsonSerializable(typeof(ProfileVerificationCodeDto))]
[JsonSerializable(typeof(ProfileVerificationCodeResultDto))]
[JsonSerializable(typeof(RamInfo))]
[JsonSerializable(typeof(RefreshTokenRequestDto))]
[JsonSerializable(typeof(RegenerateMyOAuthAppSecretRequest))]
[JsonSerializable(typeof(RegenerateOAuthAppSecretRequest))]
[JsonSerializable(typeof(RegisterRequestDto))]
[JsonSerializable(typeof(RegisterResultDto))]
[JsonSerializable(typeof(RemindRequest))]
[JsonSerializable(typeof(RemoveByPatternRequest))]
[JsonSerializable(typeof(RemoveRequest))]
[JsonSerializable(typeof(RemoveTenantSupportMemberRequest))]
[JsonSerializable(typeof(ResourceCreateDto))]
[JsonSerializable(typeof(ResourceDetailDto))]
[JsonSerializable(typeof(ResourcePageQueryDto))]
[JsonSerializable(typeof(ResourceSelectQueryDto))]
[JsonSerializable(typeof(ResourceStatusUpdateDto))]
[JsonSerializable(typeof(ResourceUpdateDto))]
[JsonSerializable(typeof(RevokeOtherSessionsRequest))]
[JsonSerializable(typeof(RevokeTenantEditionPermissionRequest))]
[JsonSerializable(typeof(RoleCreateDto))]
[JsonSerializable(typeof(RoleDataScopeDetailDto))]
[JsonSerializable(typeof(RoleDataScopeSetDto))]
[JsonSerializable(typeof(RoleDetailDto))]
[JsonSerializable(typeof(RoleHierarchyBatchUpdateDto))]
[JsonSerializable(typeof(RoleManagementDetailDto))]
[JsonSerializable(typeof(RoleMemberBatchUpdateDto))]
[JsonSerializable(typeof(RolePageQueryDto))]
[JsonSerializable(typeof(RolePermissionBatchUpdateDto))]
[JsonSerializable(typeof(RolePermissionDetailDto))]
[JsonSerializable(typeof(RolePermissionStatusUpdateDto))]
[JsonSerializable(typeof(RolePermissionUpdateDto))]
[JsonSerializable(typeof(RoleSelectQueryDto))]
[JsonSerializable(typeof(RoleStatusUpdateDto))]
[JsonSerializable(typeof(RoleUpdateDto))]
[JsonSerializable(typeof(RuntimeInfo))]
[JsonSerializable(typeof(SendEmailVerifyCodeRequest))]
[JsonSerializable(typeof(SendPhoneVerifyCodeRequest))]
[JsonSerializable(typeof(ServerInfoDto))]
[JsonSerializable(typeof(SessionRoleDetailDto))]
[JsonSerializable(typeof(SessionRolePageQueryDto))]
[JsonSerializable(typeof(Setup2FARequest))]
[JsonSerializable(typeof(SmsConfigCreateDto))]
[JsonSerializable(typeof(SmsConfigDefaultUpdateDto))]
[JsonSerializable(typeof(SmsConfigDetailDto))]
[JsonSerializable(typeof(SmsConfigPageQueryDto))]
[JsonSerializable(typeof(SmsConfigStatusUpdateDto))]
[JsonSerializable(typeof(SmsConfigUpdateDto))]
[JsonSerializable(typeof(SmsCreateDto))]
[JsonSerializable(typeof(SmsDetailDto))]
[JsonSerializable(typeof(SmsPageQueryDto))]
[JsonSerializable(typeof(SmsStatusUpdateDto))]
[JsonSerializable(typeof(SmsUpdateDto))]
[JsonSerializable(typeof(StartImpersonationRequestDto))]
[JsonSerializable(typeof(StopImpersonationRequest))]
[JsonSerializable(typeof(StorageConfigCreateDto))]
[JsonSerializable(typeof(StorageConfigDefaultUpdateDto))]
[JsonSerializable(typeof(StorageConfigDetailDto))]
[JsonSerializable(typeof(StorageConfigPageQueryDto))]
[JsonSerializable(typeof(StorageConfigStatusUpdateDto))]
[JsonSerializable(typeof(StorageConfigUpdateDto))]
[JsonSerializable(typeof(SwitchTenantRequestDto))]
[JsonSerializable(typeof(TaskCreateDto))]
[JsonSerializable(typeof(TaskDetailDto))]
[JsonSerializable(typeof(TaskLogDetailDto))]
[JsonSerializable(typeof(TaskLogPageQueryDto))]
[JsonSerializable(typeof(TaskPageQueryDto))]
[JsonSerializable(typeof(TaskRunDto))]
[JsonSerializable(typeof(TaskRunResultDto))]
[JsonSerializable(typeof(TaskRunStatusUpdateDto))]
[JsonSerializable(typeof(TaskStatusUpdateDto))]
[JsonSerializable(typeof(TaskUpdateDto))]
[JsonSerializable(typeof(TelegramBotCreateDto))]
[JsonSerializable(typeof(TelegramBotDetailDto))]
[JsonSerializable(typeof(TelegramBotPageQueryDto))]
[JsonSerializable(typeof(TelegramBotStatusUpdateDto))]
[JsonSerializable(typeof(TelegramBotUpdateDto))]
[JsonSerializable(typeof(TenantAdminInitializeDto))]
[JsonSerializable(typeof(TenantCreateDto))]
[JsonSerializable(typeof(TenantDetailDto))]
[JsonSerializable(typeof(TenantEditionCreateDto))]
[JsonSerializable(typeof(TenantEditionDefaultUpdateDto))]
[JsonSerializable(typeof(TenantEditionDetailDto))]
[JsonSerializable(typeof(TenantEditionPageQueryDto))]
[JsonSerializable(typeof(TenantEditionPermissionBatchUpdateDto))]
[JsonSerializable(typeof(TenantEditionPermissionDetailDto))]
[JsonSerializable(typeof(TenantEditionPermissionGrantDto))]
[JsonSerializable(typeof(TenantEditionPermissionStatusUpdateDto))]
[JsonSerializable(typeof(TenantEditionStatusUpdateDto))]
[JsonSerializable(typeof(TenantEditionUpdateDto))]
[JsonSerializable(typeof(TenantMemberAddDto))]
[JsonSerializable(typeof(TenantMemberDetailDto))]
[JsonSerializable(typeof(TenantMemberInviteDto))]
[JsonSerializable(typeof(TenantMemberInviteStatusUpdateDto))]
[JsonSerializable(typeof(TenantMemberPageQueryDto))]
[JsonSerializable(typeof(TenantMemberStatusUpdateDto))]
[JsonSerializable(typeof(TenantMemberUpdateDto))]
[JsonSerializable(typeof(TenantOwnerTransferDto))]
[JsonSerializable(typeof(TenantPageQueryDto))]
[JsonSerializable(typeof(TenantStatusUpdateDto))]
[JsonSerializable(typeof(TenantSubscriptionDto))]
[JsonSerializable(typeof(TenantSupportMemberAddDto))]
[JsonSerializable(typeof(TenantUpdateDto))]
[JsonSerializable(typeof(TraceTimelineQueryDto))]
[JsonSerializable(typeof(TraceTimelineResultDto))]
[JsonSerializable(typeof(UnlockSessionRequestDto))]
[JsonSerializable(typeof(UserCreateDto))]
[JsonSerializable(typeof(UserDataScopeDetailDto))]
[JsonSerializable(typeof(UserDataScopeSetDto))]
[JsonSerializable(typeof(UserDataScopeSettingDto))]
[JsonSerializable(typeof(UserDepartmentBatchUpdateDto))]
[JsonSerializable(typeof(UserDepartmentDetailDto))]
[JsonSerializable(typeof(UserDepartmentStatusUpdateDto))]
[JsonSerializable(typeof(UserDepartmentUpdateDto))]
[JsonSerializable(typeof(UserDetailDto))]
[JsonSerializable(typeof(UserInboxUpdateDto))]
[JsonSerializable(typeof(UserInfoDto))]
[JsonSerializable(typeof(UserLockUpdateDto))]
[JsonSerializable(typeof(UserLoginPolicyUpdateDto))]
[JsonSerializable(typeof(UserManagementDetailDto))]
[JsonSerializable(typeof(UserNotificationDetailDto))]
[JsonSerializable(typeof(UserNotificationPageQueryDto))]
[JsonSerializable(typeof(UserPageQueryDto))]
[JsonSerializable(typeof(UserPasswordResetDto))]
[JsonSerializable(typeof(UserPermissionBatchUpdateDto))]
[JsonSerializable(typeof(UserPermissionDetailDto))]
[JsonSerializable(typeof(UserPermissionStatusUpdateDto))]
[JsonSerializable(typeof(UserPermissionUpdateDto))]
[JsonSerializable(typeof(UserProfileDto))]
[JsonSerializable(typeof(UserRoleBatchUpdateDto))]
[JsonSerializable(typeof(UserRoleDetailDto))]
[JsonSerializable(typeof(UserRoleStatusUpdateDto))]
[JsonSerializable(typeof(UserRoleUpdateDto))]
[JsonSerializable(typeof(UserSecurityDetailDto))]
[JsonSerializable(typeof(UserSecurityPageQueryDto))]
[JsonSerializable(typeof(UserSelectQueryDto))]
[JsonSerializable(typeof(UserSessionDetailDto))]
[JsonSerializable(typeof(UserSessionPageQueryDto))]
[JsonSerializable(typeof(UserSessionRevokeDto))]
[JsonSerializable(typeof(UserSessionsRevokeDto))]
[JsonSerializable(typeof(UserSettingDto))]
[JsonSerializable(typeof(UserSettingSaveDto))]
[JsonSerializable(typeof(UserStatisticsDetailDto))]
[JsonSerializable(typeof(UserStatisticsPageQueryDto))]
[JsonSerializable(typeof(UserStatusUpdateDto))]
[JsonSerializable(typeof(UserTwoFactorResetDto))]
[JsonSerializable(typeof(UserUpdateDto))]
[JsonSerializable(typeof(VerificationCodeResultDto))]
[JsonSerializable(typeof(VersionDetailDto))]
[JsonSerializable(typeof(VersionPageQueryDto))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
internal sealed partial class SaasOperationsJsonContext : JsonSerializerContext;
