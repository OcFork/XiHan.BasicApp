import type { Router } from 'vue-router'
import type { EnumMetadata } from '@/api/modules/metadata/enum-metadata'
import type {
  ApiCredentialItem,
  ApiCredentialSecret,
  AppBackendDependency,
  AppEnumBatchQuery,
  AppEnumDefinition,
  AppEnumNameQuery,
  CaptchaChallenge,
  ChangeEmailParams,
  ChangePasswordParams,
  ChangePhoneParams,
  ChangeUserNameParams,
  EmailLoginParams,
  ExternalLoginItem,
  ImpersonationCandidateQuery,
  LoginConfig,
  LoginLogPage,
  LoginParams,
  LoginResponse,
  LoginToken,
  MyOAuthAppCreateInput,
  MyOAuthAppItem,
  MyOAuthAppSecret,
  MyOAuthAppUpdateInput,
  NotificationPreference,
  PasswordResetConfirmResult,
  PasswordResetResult,
  PermissionInfo,
  PhoneLoginParams,
  StartImpersonationParams,
  SwitchTenantParams,
  TwoFactorSetupResult,
  UpdateProfileParams,
  UserActivity,
  UserInfo,
  UserProfile,
  UserSessionItem,
  VerificationCodeResult,
} from '~/types'
import { createDynamicApiClient } from '@/api/base'
import { ResourceAccessLevel } from '@/api/modules/authorization'
import { dictApi } from '@/api/modules/configuration'
import { fileApi } from '@/api/modules/files'
import { impersonationApi } from '@/api/modules/identity'
import { enumMetadataApi } from '@/api/modules/metadata/enum-metadata'
import { tenantApi } from '@/api/modules/tenant'
import { workbenchApi } from '@/api/modules/workbench'
import { router } from '@/router'
import { staticRoutes } from '@/router/routes'
import { registerAppContext } from '~/stores/app-context'

const appViewModules = import.meta.glob('/src/views/**/*.vue')
// 模块视图（src/modules/<模块>/views/**）：键重写为 /src/views/** 形态并入同一张表，
// 后端菜单的 Component 路径无需感知模块归属；同键冲突以模块侧为准并在控制台报错（构建门禁 validate-modules 兜底）。
const moduleViewModules = import.meta.glob('/src/modules/*/views/**/*.vue')
const viewModules: Record<string, () => Promise<unknown>> = { ...appViewModules }
for (const [path, loader] of Object.entries(moduleViewModules)) {
  const key = path.replace(/^\/src\/modules\/[^/]+\/views/, '/src/views')
  if (key in viewModules)
    console.error(`[modules] 视图键冲突：${key} 已存在，${path} 将覆盖它`)
  viewModules[key] = loader
}

const defaultLoginConfig: LoginConfig = {
  loginMethods: ['password'],
  oAuthProviders: [],
  captchaEnabled: false,
}

function emptyEnum(name: string): AppEnumDefinition {
  return {
    // cultureName 目前无消费者：全库仅此处与 mapEnumMetadata 赋值，类型里有声明但没人读。
    // 枚举文案实际由后端按 X-Language 解析后直接返回，前端不据此二次判断，故保持常量。
    cultureName: 'zh-CN',
    displayName: name,
    enumName: name,
    fullName: name,
    isFlags: false,
    items: [],
    underlyingTypeName: 'Int32',
  }
}

// 将后端枚举元数据（EnumMetadata）映射为前端枚举定义（AppEnumDefinition）。
// 后端业务枚举均为 Int32 底层、非 Flags；显示文案由后端按 X-Language 文化解析。
// 关键：后端实例数据经 JsonStringEnumConverter 序列化为「成员名字符串」（如 "Field"/"Pending"），
// 故选项 value 必须用成员名 item.name，才能与表格行数据匹配；item.value(整数)放入 valueText 备用。
function mapEnumMetadata(meta: EnumMetadata): AppEnumDefinition {
  return {
    cultureName: 'zh-CN',
    displayName: meta.displayName || meta.enumTypeName,
    enumName: meta.enumTypeName,
    fullName: meta.enumTypeName,
    isFlags: false,
    underlyingTypeName: 'Int32',
    items: (meta.items ?? []).map((item, index) => ({
      name: item.name,
      value: item.name,
      valueText: String(item.value),
      label: item.displayName || item.name,
      description: item.description ?? '',
      order: index,
      disabled: false,
      source: 'enum',
    })),
  }
}

/** 按控制器缓存的动态客户端：context 里的调用都是 4.x 的「控制器/动作」写法，经映射表落到 5.0 操作 */
const clients = new Map<string, ReturnType<typeof createDynamicApiClient>>()
function api(controller: string) {
  let client = clients.get(controller)
  if (!client) {
    client = createDynamicApiClient(controller)
    clients.set(controller, client)
  }
  return client
}

async function getWithFallback<T>(request: () => Promise<T>, fallback: T): Promise<T> {
  try {
    return await request()
  }
  catch {
    return fallback
  }
}

async function postWithFallback<T>(request: () => Promise<T>, fallback: T): Promise<T> {
  try {
    return await request()
  }
  catch {
    return fallback
  }
}

function createAuthApis() {
  return {
    getLoginConfigApi() {
      return getWithFallback<LoginConfig>(() => api('Auth').get<LoginConfig>('LoginConfig'), defaultLoginConfig)
    },
    getPermissionsApi() {
      return api('Auth').get<PermissionInfo>('Permissions')
    },
    getUserInfoApi() {
      return api('Auth').get<UserInfo>('UserInfo')
    },
    loginApi(input: LoginParams) {
      return api('Auth').post<LoginResponse>('Login', input)
    },
    getCaptchaApi() {
      return api('Auth').get<CaptchaChallenge>('Captcha')
    },
    logoutApi() {
      return postWithFallback(() => api('Auth').post('Logout'), undefined)
    },
    lockSessionApi(input: { password: string }) {
      return api('Auth').post<unknown>('LockSession', input)
    },
    unlockSessionApi(input: { password: string }) {
      return api('Auth').post<unknown>('UnlockSession', input)
    },
    phoneLoginApi(input: PhoneLoginParams) {
      return api('Auth').post<LoginToken>('PhoneLogin', input)
    },
    emailLoginApi(input: EmailLoginParams) {
      return api('Auth').post<LoginToken>('EmailLogin', input)
    },
    sendEmailLoginCodeApi(email: string) {
      return api('Auth').post<VerificationCodeResult>('EmailLoginCode', { email })
    },
    registerApi(input: unknown) {
      return api('Auth').post('Register', input)
    },
    createOAuthBindTicketApi() {
      // 动态 API 约定会剥离方法名的 Create 前缀，实际路由为 /Auth/OAuthBindTicket
      return api('Auth').post<string>('OAuthBindTicket')
    },
    requestPasswordResetApi(email: string) {
      return api('Auth').post<PasswordResetResult>('PasswordResetRequest', { email })
    },
    consumePasswordResetTokenApi(token: string, newPassword: string) {
      return api('Auth').post<PasswordResetConfirmResult>('ConsumePasswordResetToken', { token, newPassword })
    },
    sendPhoneLoginCodeApi(phone: string) {
      return api('Auth').post<VerificationCodeResult>('PhoneLoginCode', { phone })
    },
  }
}

function createProfileApis() {
  return {
    changePasswordApi(input: ChangePasswordParams) {
      return api('Profile').post('ChangePassword', input)
    },
    changeUserNameApi(input: ChangeUserNameParams) {
      return api('Profile').post('ChangeUserName', input)
    },
    confirmChangeEmailApi(code: string) {
      return api('Profile').post('ConfirmChangeEmail', { code })
    },
    confirmChangePhoneApi(code: string) {
      return api('Profile').post('ConfirmChangePhone', { code })
    },
    deactivateAccountApi(password: string) {
      return api('Profile').post('DeactivateAccount', { password })
    },
    deleteAccountApi(password: string) {
      return api('Profile').post('DeleteAccount', { password })
    },
    disable2FAApi(method: number | string, code: string) {
      return api('Profile').post('Disable2FA', { code, method })
    },
    enable2FAApi(method: number | string, code: string) {
      return api('Profile').post('Enable2FA', { code, method })
    },
    getActivityApi() {
      return api('Profile').get<UserActivity>('Activity')
    },
    getApiCredentialsApi() {
      return api('Profile').get<ApiCredentialItem[]>('ApiCredentials')
    },
    createApiCredentialApi(credentialName?: string) {
      return api('Profile').post<ApiCredentialSecret>('ApiCredential', { credentialName })
    },
    rotateApiCredentialSecretApi(id: number | string) {
      return api('Profile').post<ApiCredentialSecret>('RotateApiCredentialSecret', { id })
    },
    updateApiCredentialStatusApi(id: number | string, status: 'Disabled' | 'Enabled') {
      return api('Profile').put<ApiCredentialItem>('ApiCredentialStatus', { id, status })
    },
    deleteApiCredentialApi(id: number | string) {
      return api('Profile').delete('ApiCredential', { id })
    },
    getMyOAuthAppsApi() {
      return api('MyOAuthApp').get<MyOAuthAppItem[]>('MyOAuthApps')
    },
    createMyOAuthAppApi(input: MyOAuthAppCreateInput) {
      return api('MyOAuthApp').post<MyOAuthAppSecret>('MyOAuthApp', input)
    },
    updateMyOAuthAppApi(input: MyOAuthAppUpdateInput) {
      return api('MyOAuthApp').put<MyOAuthAppItem>('MyOAuthApp', input)
    },
    regenerateMyOAuthAppSecretApi(id: number | string) {
      return api('MyOAuthApp').post<MyOAuthAppSecret>('RegenerateMyOAuthAppSecret', undefined, { params: { id } })
    },
    updateMyOAuthAppStatusApi(id: number | string, status: 'Disabled' | 'Enabled') {
      return api('MyOAuthApp').put<MyOAuthAppItem>('MyOAuthAppStatus', { id, status })
    },
    deleteMyOAuthAppApi(id: number | string) {
      return api('MyOAuthApp').delete('MyOAuthApp', { id })
    },
    getNotificationPreferenceApi() {
      return api('Profile').get<NotificationPreference>('NotificationPreference')
    },
    updateNotificationPreferenceApi(input: NotificationPreference) {
      return api('Profile').put<NotificationPreference>('NotificationPreference', input)
    },
    getFilePresignedUrlApi(fileId: string) {
      return fileApi.generatePresignedUrl(fileId)
    },
    // 访问级别与存储目录属应用策略，由 src 决定；契约层只承诺"上传头像 → 拿到文件主键"，
    // 不把 ResourceAccessLevel 这类纯业务枚举泄漏进 packages
    async uploadAvatarApi(file: File, onProgress?: (percent: number) => void) {
      const detail = await fileApi.upload(
        { file, accessLevel: ResourceAccessLevel.Public, directory: 'avatars' },
        onProgress,
      )
      return { fileId: detail.id }
    },
    getLinkedAccountsApi() {
      return api('Profile').get<ExternalLoginItem[]>('LinkedAccounts')
    },
    getLoginLogsApi(page: number, pageSize: number) {
      return api('Profile').get<LoginLogPage>('LoginLogs', { page, pageSize }).then(result => ({ ...result, page, pageSize }))
    },
    getProfileApi() {
      return api('Profile').get<UserProfile>('Profile')
    },
    getSessionsApi() {
      return api('Profile').get<UserSessionItem[]>('Sessions')
    },
    revokeOtherSessionsApi() {
      return api('Profile').post('RevokeOtherSessions')
    },
    revokeSessionApi(sessionId: string) {
      return api('Profile').post('RevokeSession', { sessionId })
    },
    send2FASetupCodeApi(method: number | string) {
      return api('Profile').post<VerificationCodeResult>('Send2FASetupCode', { method })
    },
    sendChangeEmailCodeApi(input: ChangeEmailParams) {
      return api('Profile').post<VerificationCodeResult>('SendChangeEmailCode', input)
    },
    sendChangePhoneCodeApi(input: ChangePhoneParams) {
      return api('Profile').post<VerificationCodeResult>('SendChangePhoneCode', input)
    },
    sendEmailVerifyCodeApi() {
      return api('Profile').post<VerificationCodeResult>('SendEmailVerifyCode')
    },
    sendPhoneVerifyCodeApi() {
      return api('Profile').post<VerificationCodeResult>('SendPhoneVerifyCode')
    },
    setup2FAApi() {
      return api('Profile').post<TwoFactorSetupResult>('Setup2FA')
    },
    unlinkAccountApi(provider: string) {
      return api('Profile').post('UnlinkAccount', { provider })
    },
    updateProfileApi(input: UpdateProfileParams) {
      return api('Profile').put<UserProfile>('Profile', input)
    },
    verifyEmailApi(code: string) {
      return api('Profile').post('VerifyEmail', { code })
    },
    verifyPhoneApi(code: string) {
      return api('Profile').post('VerifyPhone', { code })
    },
  }
}

function createShellApis() {
  return {
    enumApi: {
      // 复用既有可用端点 /EnumMetadata/AllEnums（DynamicApi 约定去掉 Get 前缀），按名筛选并映射为
      // AppEnumDefinition；淘汰原先 404 的 /Enum/Batch、/Enum/ByName 重复实现。结果由 useEnumService 缓存。
      async getBatch(query: AppEnumBatchQuery) {
        const names = query.enumNames ?? []
        const fallback = Object.fromEntries(names.map(name => [name, emptyEnum(name)]))
        try {
          const all = await enumMetadataApi.getAll()
          const wanted = new Set(names)
          const result: Record<string, AppEnumDefinition> = {}
          for (const meta of all) {
            if (wanted.size === 0 || wanted.has(meta.enumTypeName)) {
              result[meta.enumTypeName] = mapEnumMetadata(meta)
            }
          }
          // 保证请求的每个名称都有值（缺失回退空定义），维持「键齐全」契约
          for (const name of names) {
            result[name] ??= emptyEnum(name)
          }
          return result
        }
        catch {
          return fallback
        }
      },
      async getByName(query: AppEnumNameQuery) {
        const name = query.enumName
        try {
          const all = await enumMetadataApi.getAll()
          const meta = all.find(item => item.enumTypeName === name)
          return meta ? mapEnumMetadata(meta) : emptyEnum(name)
        }
        catch {
          return emptyEnum(name)
        }
      },
    },
    userSettingApi: {
      get(input: { scene: number, settingKey: string }) {
        return getWithFallback<{ scene: number, settingKey: string, settingValue?: null | string }>(
          () => api('UserSettingQuery').get('Get', input),
          { scene: input.scene, settingKey: input.settingKey, settingValue: null },
        )
      },
      save(input: { scene: number, settingKey: string, settingValue?: null | string, clientId?: string, origin?: null | string }) {
        return api('UserSetting').post<{ scene: number, settingKey: string, settingValue?: null | string }>('Save', input)
      },
    },
    dictApi: {
      options(dictCode: string) {
        return dictApi.options(dictCode)
      },
    },
    timeZoneApi: {
      options() {
        return getWithFallback<Array<{ id: string, displayName: string, baseUtcOffsetMinutes: number, supportsDaylightSavingTime: boolean }>>(
          () => api('TimeZoneMetadata').get('TimeZoneOptions'),
          [],
        )
      },
    },
    importHistoryApi: {
      create(input: { pageCode: string, resourceCode?: null | string, fileName: string, totalCount: number, successCount: number, failCount: number, errorSummary?: null | string }) {
        return api('ImportHistory').post<unknown>('Create', input)
      },
      recent(pageCode: string, count = 10) {
        return getWithFallback<Array<{ id: number | string, pageCode: string, resourceCode?: null | string, fileName: string, totalCount: number, successCount: number, failCount: number, errorSummary?: null | string, createdAt: string }>>(
          () => api('ImportHistoryQuery').get('Mine', { pageCode, count }),
          [],
        )
      },
    },
    exportTaskApi: {
      submit(input: { businessType: string, taskName?: string, scope: number, format: number, querySnapshot?: null | string, columns: Array<{ key: string, title: string, valueMap?: Record<string, string> }> }) {
        return api('ExportTask').post<unknown>('Submit', input)
      },
    },
    serverApi: {
      getNuGetPackages() {
        return getWithFallback<AppBackendDependency[]>(() => api('Server').get<AppBackendDependency[]>('NuGetPackages'), [])
      },
    },
    userInboxApi: {
      banner() {
        return workbenchApi.inbox.banner()
      },
      confirm(id: string, _userId?: string, _tenantId?: null | string) {
        return workbenchApi.inbox.confirm(id)
      },
      list(_userId?: string, unreadOnly = false, _tenantId?: null | string) {
        return workbenchApi.inbox.list(unreadOnly)
      },
      mandatoryUnread() {
        return workbenchApi.inbox.mandatoryUnread()
      },
      markAllRead(_userId?: string, _tenantId?: null | string) {
        return workbenchApi.inbox.markAllRead()
      },
      markPopupShown(id: string) {
        return workbenchApi.inbox.markPopupShown(id)
      },
      markRead(id: string, _userId?: string, _tenantId?: null | string) {
        return workbenchApi.inbox.markRead(id)
      },
      popup() {
        return workbenchApi.inbox.popup()
      },
    },
  }
}

function createImpersonationApis() {
  return {
    impersonationApi: {
      tenants: () => impersonationApi.tenants(),
      candidates: (input: ImpersonationCandidateQuery) => impersonationApi.candidates(input),
      start: (input: StartImpersonationParams) => impersonationApi.start(input),
      stop: () => impersonationApi.stop(),
    },
  }
}

function createTenantApis() {
  return {
    tenantApi: {
      myAvailableTenants: () => tenantApi.myAvailableTenants(),
      switchTenant: (input: SwitchTenantParams) => tenantApi.switchTenant(input),
    },
  }
}

export function createApplicationApis() {
  return {
    ...createAuthApis(),
    ...createProfileApis(),
    ...createImpersonationApis(),
    ...createShellApis(),
    ...createTenantApis(),
  }
}

export function registerApplicationContext(appRouter: Router = router) {
  registerAppContext({
    apis: createApplicationApis(),
    explicitComponentMap: {},
    getRouter: () => Promise.resolve(appRouter),
    getStaticRoutes: () => staticRoutes,
    viewModules,
    // shell 要跳转、但由本应用（路由表 / 后端 PageRegistry）定义的路径
    shellRoutes: {
      profile: '/workbench/profile',
      controlCenter: '/control-center',
      inbox: '/workbench/inbox',
    },
  })
}
