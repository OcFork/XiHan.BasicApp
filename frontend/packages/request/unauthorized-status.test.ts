/**
 * 401 分支判定依据的回归测试。
 *
 * 职责边界：只锁定一件事——响应错误拦截器判定「要不要刷新令牌」时比的是 **HTTP 状态码**，
 * 不看响应体。5.0 的错误响应是 Problem Details，code 是字符串错误码（与 HTTP 状态码无关），
 * 本文件让 401 响应带一个与 401 毫不相干的 code，任何「按响应体里的码判 401」的写法都会立刻失效。
 * 刷新链路本身的分支在 auth.test.ts 里。
 */
import type { AxiosAdapter, InternalAxiosRequestConfig } from 'axios'
import type { Router } from 'vue-router'
import { AxiosError, AxiosHeaders } from 'axios'
import { beforeEach, expect, it } from 'vitest'
import { REFRESH_TOKEN_KEY, TOKEN_KEY } from '~/constants'
import { LocalStorage } from '~/utils'
import { bindLockHook, bindLogoutHook, bindRouter, RequestClient } from './index'

/** 401 响应的 Problem Details：code 故意取一个看不出是 401 的字符串错误码 */
const UNAUTHORIZED_PROBLEM = {
  type: 'about:blank',
  title: 'Unauthorized',
  status: 401,
  code: 'SECURITY_TOKEN_001',
}

beforeEach(() => {
  bindRouter({ replace: () => Promise.resolve() } as unknown as Router)
  bindLogoutHook(() => {})
  bindLockHook(() => {})
})

it('hTTP 401 只凭状态码触发刷新与重放，响应体里的错误码不参与判定', async () => {
  LocalStorage.set(TOKEN_KEY, 'old-access')
  LocalStorage.set(REFRESH_TOKEN_KEY, 'old-refresh')
  let refreshCount = 0
  const client = new RequestClient({
    adapter: ((config: InternalAxiosRequestConfig) => {
      if (String(config.url).endsWith('/auth:refresh-token')) {
        refreshCount += 1
        return Promise.resolve({
          config,
          data: { accessToken: 'new-access' },
          headers: new AxiosHeaders(),
          status: 200,
          statusText: 'OK',
        })
      }
      return String(config.headers.get('Authorization')) === 'Bearer new-access'
        ? Promise.resolve({
            config,
            data: { id: 1 },
            headers: new AxiosHeaders(),
            status: 200,
            statusText: 'OK',
          })
        : Promise.reject(new AxiosError(
            'Request failed with status code 401',
            AxiosError.ERR_BAD_RESPONSE,
            config,
            null,
            { config, data: UNAUTHORIZED_PROBLEM, headers: new AxiosHeaders(), status: 401, statusText: '' },
          ))
    }) as unknown as AxiosAdapter,
  })

  await expect(client.get('/api/user/1')).resolves.toEqual({ id: 1 })
  expect(refreshCount).toBe(1)
})
