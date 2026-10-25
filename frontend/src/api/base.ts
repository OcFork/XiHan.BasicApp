import type { OperationMethod, OperationRoute } from './operations.generated'
import type { ApiId, ApiPrimitive, PageRequest, PageResult } from './types'
import type { AxiosRequestConfig } from '~/request'
import { OPERATIONS } from './operations.generated'
import { requestClient } from './request'

export type DynamicApiParamValue = boolean | number | string | undefined
export type DynamicApiParams = Record<string, DynamicApiParamValue>
/** facade 调用时写的 4.x 动态 API 谓词：只用来在同名动作之间区分操作，真正发出的方法以 5.0 路由为准 */
export type DynamicApiVerb = OperationMethod

export interface DynamicApiClient {
  delete: <TResult = void>(action: string, params?: DynamicApiParams, config?: AxiosRequestConfig) => Promise<TResult>
  get: <TResult>(action: string, params?: DynamicApiParams, config?: AxiosRequestConfig) => Promise<TResult>
  post: <TResult, TBody = unknown>(action: string, body?: TBody, config?: AxiosRequestConfig) => Promise<TResult>
  put: <TResult, TBody = unknown>(action: string, body?: TBody, config?: AxiosRequestConfig) => Promise<TResult>
}

export interface ReadApi<TListItem, TDetail, TQuery extends PageRequest = PageRequest> {
  page: (input: TQuery) => Promise<PageResult<TListItem>>
  detail: (id: ApiId) => Promise<TDetail | null>
}

export interface CommandApi<TCreate, TUpdate, TDetail> {
  create: (input: TCreate) => Promise<TDetail>
  update: (input: TUpdate) => Promise<TDetail>
}

/**
 * 把 facade 的「控制器/动作」解析成 5.0 操作路由（表由 scripts/generate-operations.mjs 从后端源码生成）。
 *
 * 动作只对应一个操作时直接用它（4.x 的 [HttpPost] 等标注让 facade 的谓词与方法名前缀不一定一致）；
 * 同名动作对应多个操作（如 User/User 的新增、更新、删除）时按谓词挑。找不到就抛错——
 * 路由对不上就是 404，宁可在开发期暴露，也不要静默拼出一个旧式 URL。
 */
export function resolveOperationRoute(verb: DynamicApiVerb, controllerName: string, action: string): OperationRoute {
  const target = `${normalizeSegment(controllerName)}/${normalizeSegment(action)}`
  const entry = OPERATIONS[target]
  if (!entry) {
    throw new Error(`Dynamic API 调用 ${verb} ${target} 没有对应的 5.0 操作：核对控制器 / 动作名，或在后端新增操作后执行 pnpm api:operations`)
  }
  const routes = Object.values(entry)
  if (routes.length === 1) {
    return routes[0]!
  }
  const route = entry[verb]
  if (!route) {
    throw new Error(`Dynamic API 调用 ${verb} ${target} 对应多个 5.0 操作（${routes.map(item => item.id).join('、')}），没有一个是 ${verb}`)
  }
  return route
}

/**
 * 按路由模板拼出最终 URL：实例级路由从调用参数里取 `id` 填进 `{id}`（依次看各个来源，取第一个非空值）。
 * 映射表里的路径已带前缀，request 层识别到前缀后不会再补一次。
 */
export function buildOperationUrl(route: OperationRoute, ...sources: unknown[]): string {
  if (!route.path.includes('{id}')) {
    return route.path
  }
  for (const source of sources) {
    const id = readIdentifier(source)
    if (id !== undefined) {
      return route.path.replace('{id}', formatDynamicApiRouteValue(id))
    }
  }
  throw new Error(`操作 ${route.id} 是实例级路由（${route.path}），调用时必须提供 id`)
}

function readIdentifier(source: unknown): ApiId | undefined {
  if (!isPlainParams(source)) {
    return undefined
  }
  const value = source.id
  if (value === undefined || value === null || value === '') {
    return undefined
  }
  return String(value)
}

/** 有请求体的方法：读取类调用落到这些路由时，参数以 JSON 请求体上送（保留布尔与数字的类型） */
function carriesBody(method: OperationMethod) {
  return method === 'POST' || method === 'PUT'
}

function send<TResult>(route: OperationRoute, url: string, body: unknown, config: AxiosRequestConfig) {
  switch (route.method) {
    case 'GET':
      return requestClient.get<TResult>(url, config)
    case 'DELETE':
      return requestClient.delete<TResult>(url, config)
    case 'PUT':
      return requestClient.put<TResult>(url, body, config)
    default:
      return requestClient.post<TResult>(url, body, config)
  }
}

export function createDynamicApiClient(controllerName: string): DynamicApiClient {
  const normalizedControllerName = normalizeSegment(controllerName)

  /**
   * 读取类调用（facade 写的 GET / DELETE）：参数表落到 GET / DELETE 路由时走查询串，
   * 落到 POST / PUT 路由时作为 JSON 请求体（5.0 的读操作多数是集合级 POST 动词）
   */
  function read<TResult>(verb: DynamicApiVerb, action: string, params: DynamicApiParams | undefined, config: AxiosRequestConfig | undefined) {
    const route = resolveOperationRoute(verb, normalizedControllerName, action)
    const merged = mergeDynamicApiParams(config?.params, params)
    const url = buildOperationUrl(route, merged)
    if (carriesBody(route.method)) {
      const { params: _ignored, ...rest } = config ?? {}
      return send<TResult>(route, url, merged ?? {}, rest)
    }
    return send<TResult>(route, url, undefined, { ...config, params: merged })
  }

  /**
   * 写入类调用（facade 写的 POST / PUT）：请求对象作 JSON 请求体，config.params 照常走查询串；
   * 落到 GET / DELETE 路由（不读请求体）时把请求对象的标量字段并进查询串
   */
  function write<TResult, TBody>(verb: DynamicApiVerb, action: string, body: TBody | undefined, config: AxiosRequestConfig | undefined) {
    const route = resolveOperationRoute(verb, normalizedControllerName, action)
    const url = buildOperationUrl(route, body, config?.params)
    if (carriesBody(route.method)) {
      return send<TResult>(route, url, body, config ?? {})
    }
    return send<TResult>(route, url, undefined, { ...config, params: mergeDynamicApiParams(config?.params, toQueryParams(body)) })
  }

  return {
    get<TResult>(action: string, params?: DynamicApiParams, config?: AxiosRequestConfig) {
      return read<TResult>('GET', action, params, config)
    },
    post<TResult, TBody = unknown>(action: string, body?: TBody, config?: AxiosRequestConfig) {
      return write<TResult, TBody>('POST', action, body, config)
    },
    put<TResult, TBody = unknown>(action: string, body?: TBody, config?: AxiosRequestConfig) {
      return write<TResult, TBody>('PUT', action, body, config)
    },
    delete<TResult = void>(action: string, params?: DynamicApiParams, config?: AxiosRequestConfig) {
      return read<TResult>('DELETE', action, params, config)
    },
  }
}

export function createReadApi<TListItem, TDetail, TQuery extends PageRequest = PageRequest>(
  controllerName: string,
  resourceName: string,
): ReadApi<TListItem, TDetail, TQuery> {
  const api = createDynamicApiClient(controllerName)
  const normalizedResourceName = normalizeSegment(resourceName)

  return {
    page(input: TQuery) {
      // 分页统一以整个查询对象作 JSON 请求体
      return api.post<PageResult<TListItem>, TQuery>(`${normalizedResourceName}Page`, input)
    },
    detail(id: ApiId) {
      return api.get<TDetail | null>(`${normalizedResourceName}Detail`, { id })
    },
  }
}

export function createCommandApi<TCreate, TUpdate, TDetail>(
  controllerName: string,
  resourceName: string,
): CommandApi<TCreate, TUpdate, TDetail> {
  const api = createDynamicApiClient(controllerName)
  const normalizedResourceName = normalizeSegment(resourceName)

  return {
    create(input: TCreate) {
      return api.post<TDetail, TCreate>(normalizedResourceName, input)
    },
    update(input: TUpdate) {
      return api.put<TDetail, TUpdate>(normalizedResourceName, input)
    },
  }
}

/**
 * 合并两条参数通道：params 位与 config.params。
 *
 * params 位优先（同名键覆盖 config.params），但不因为 params 位没传就把 config.params 整段清掉——
 * `client.get(action, undefined, { params })` 不能静默丢掉查询条件。两侧都没给时返回 undefined，保持「不带参数」的原状。
 */
function mergeDynamicApiParams(configParams: unknown, params: DynamicApiParams | undefined) {
  if (params === undefined) {
    return configParams as DynamicApiParams | undefined
  }
  // URLSearchParams 等非普通对象不做展开（展开会得到空对象），此时仍以 params 位为准
  if (!isPlainParams(configParams)) {
    return params
  }
  return { ...configParams, ...params } as DynamicApiParams
}

/** 写入调用落到 GET / DELETE 路由时，把请求对象的标量字段拆成查询串；嵌套对象进不了查询串，直接报错而不是静默丢字段 */
function toQueryParams(body: unknown): DynamicApiParams | undefined {
  if (body === undefined || body === null) {
    return undefined
  }
  if (!isPlainParams(body)) {
    throw new TypeError('落到 GET / DELETE 路由的调用只能传普通对象作为参数')
  }
  const params: DynamicApiParams = {}
  for (const [key, value] of Object.entries(body)) {
    if (value === undefined || value === null) {
      continue
    }
    if (typeof value === 'object') {
      throw new TypeError(`字段 ${key} 是对象，无法放进 GET / DELETE 路由的查询串`)
    }
    params[key] = value as DynamicApiParamValue
  }
  return params
}

function isPlainParams(value: unknown): value is Record<string, unknown> {
  if (typeof value !== 'object' || value === null) {
    return false
  }
  const prototype: unknown = Object.getPrototypeOf(value)
  return prototype === Object.prototype || prototype === null
}

// 非分页的下拉/统计等读取接口用它拼装简单参数：空值不写入
export function appendDynamicApiParam(params: DynamicApiParams, key: string, value: ApiPrimitive | undefined) {
  if (value === undefined || value === null || value === '') {
    return
  }

  params[key] = value
}

export function formatDynamicApiRouteValue(value: ApiId) {
  return encodeURIComponent(value)
}

function normalizeSegment(value: string) {
  const normalized = value.trim().replace(/^\/+|\/+$/g, '')
  if (!normalized) {
    throw new Error('Dynamic API 路由段不能为空')
  }
  return normalized
}
