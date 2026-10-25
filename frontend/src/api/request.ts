import type { RequestClient } from '~/request'
import { createRequestClient } from '~/request'
import { OPERATIONS } from './operations.generated'

const baseURL = String(import.meta.env.VITE_API_BASE_URL ?? '')
const apiPrefix = String(import.meta.env.VITE_API_PREFIX ?? '/api')
// 刷新令牌不经动态客户端（它在拦截器里发），路径直接从映射表取，后端改名时随表更新
const refreshTokenUrl = Object.values(OPERATIONS['Auth/RefreshToken'] ?? {})[0]?.path

export const requestClient: RequestClient = createRequestClient(baseURL, apiPrefix, refreshTokenUrl)

export type { RequestClient }
