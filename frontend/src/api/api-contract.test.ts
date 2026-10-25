import type { OperationRoute } from './operations.generated'
/**
 * 全量 API 契约结构化测试。
 *
 * 职责边界：不逐个模块写用例，而是把 src/api/modules 与 src/modules/<模块>/api 下
 * 全部 `*Api` 门面对象一次性遍历、逐个方法探针调用，用记录下来的「HTTP 谓词 + URL + 参数位置」
 * 断言全仓统一的 5.0 路由约定：/api/<资源>[/{id}][:<动词>]，资源与动词都是 kebab-case，
 * 每条请求都能在 operations.generated.ts 里找到对应的操作路由。任何新增接口只要违反约定就会在这里失败。
 * 不发真实请求：requestClient 被整体替换为记录器。
 */
import type { AxiosRequestConfig } from '~/request'
import { beforeAll, describe, expect, it, vi } from 'vitest'
import { OPERATIONS } from './operations.generated'

interface RecordedCall {
  method: 'DELETE' | 'GET' | 'POST' | 'PUT'
  url: string
  body?: unknown
  params?: unknown
}

const hoisted = vi.hoisted(() => ({ calls: [] as RecordedCall[] }))

vi.mock('@/api/request', () => ({
  requestClient: {
    get(url: string, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'GET', url, params: config?.params })
      return Promise.resolve(null)
    },
    post(url: string, body?: unknown, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'POST', url, body, params: config?.params })
      return Promise.resolve(null)
    },
    put(url: string, body?: unknown, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'PUT', url, body, params: config?.params })
      return Promise.resolve(null)
    },
    delete(url: string, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'DELETE', url, params: config?.params })
      return Promise.resolve(undefined)
    },
  },
}))

/** 零代码只读运行时的分页：全仓唯一一条走 GET + 查询串的分页（请求是扁平的），其余一律 POST + body */
const READONLY_RUNTIME_PAGE = '/api/dynamic-runtime'

/** 5.0 路由形状：/api/<资源>，可选实例段 /{id}，可选 :<动词>；资源与动词都是 kebab-case */
const ROUTE_SHAPE = /^\/api\/[a-z][a-z0-9-]*(?:\/[^/:?#]+)?(?::[a-z][a-z0-9-]*)?$/

/**
 * 已知的「同一门面内同谓词同 URL」重复项，正常应为空。
 *
 * 回归锚点：曾经 userApi.resetPassword 与 userSecurityApi.resetPassword 都打同一个重置密码端点，
 * 被 userManagementApi 同时展开（`...userApi` + `security: userSecurityApi`）后在一个门面里出现两个等价入口、
 * 且两者声明的返回 DTO 不同。前者已删除，这里锁定「不再有重复路由」；新增重复项必须先消除，
 * 而不是往这张清单里追加。
 */
const KNOWN_DUPLICATE_ROUTES: string[] = []

/** 分页探针入参：用同一个对象实例，便于断言「原样作 body 上送」而不是被拆成查询串 */
const PAGE_QUERY = {
  conditions: { filters: [], keyword: null, sorts: [] },
  page: { pageIndex: 1, pageSize: 20 },
}

/** 探针主键：标量入参直接用它，对象入参包成 { id } */
const PROBE_ID = 'probe-1'

/** 一次探针调用的结果：门面来源 + 方法名 + 记录到的请求 */
interface Endpoint extends RecordedCall {
  /** 门面导出名，如 userApi / permissionCenterApi */
  facade: string
  /** 门面内的访问路径，如 detail / inbox.markRead */
  path: string
  /** 是否是退回 { id } 入参后的第二次调用；这时请求里的 id 是探针自己塞的，不代表门面的写法 */
  retried: boolean
}

const endpoints: Endpoint[] = []
/** 探针调用后没有发出任何请求的方法（理应为空） */
const silent: string[] = []
/** 两种入参形态下都直接抛错的方法（理应为空） */
const failed: string[] = []

/** 操作表里全部路由，按「谓词 + 路径模板」索引 */
const knownRoutes = new Set(
  Object.values(OPERATIONS)
    .flatMap(entry => Object.values(entry) as OperationRoute[])
    .map(route => `${route.method} ${route.path}`),
)

function lastName(path: string) {
  return path.split('.').at(-1) ?? ''
}

function isPageLike(name: string) {
  return name === 'page' || name.endsWith('Page')
}

function isPlainObject(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null && !Array.isArray(value)
}

/** DynamicApiClient 本体（get/post/put/delete 四件套）不是业务门面，不参与探针 */
function isDynamicApiClient(value: Record<string, unknown>) {
  return ['get', 'post', 'put', 'delete'].every(key => typeof value[key] === 'function')
}

/** 地址里的实例段（/api/<资源>/<段>[:动词] 的 <段>），集合级路由返回 undefined */
function instanceSegment(url: string) {
  return /^\/api\/[^/]+\/([^/:]+)/.exec(url)?.[1]
}

/** 地址里的动词（冒号之后），标准动词路由返回 undefined */
function customVerb(url: string) {
  return /:([a-z0-9-]+)$/.exec(url)?.[1]
}

/** 把实例段还原成 {id}，得到可与操作表比对的路径模板 */
function toTemplate(url: string) {
  return url.replace(/^(\/api\/[^/]+)\/[^/:]+/, '$1/{id}')
}

function invoke(fn: (...args: unknown[]) => unknown, firstArg: unknown): { error?: unknown, returned?: unknown } {
  try {
    return { returned: fn(firstArg, 'probe-2', 'probe-3') }
  }
  catch (error) {
    return { error }
  }
}

function probe(facade: string, path: string, fn: (...args: unknown[]) => unknown) {
  const before = hoisted.calls.length
  const pageLike = isPageLike(lastName(path))
  // 先按标量主键调；实例级路由要求入参带 id、落到 GET / DELETE 的调用要求普通对象，
  // 标量探针在这两类上必然抛错，于是退回 { id } 形态的入参再调一次——这正是调用方会传的两种形态。
  let attempt = invoke(fn, pageLike ? PAGE_QUERY : PROBE_ID)
  const retried = attempt.error !== undefined && !pageLike
  if (retried) {
    hoisted.calls.length = before
    attempt = invoke(fn, { id: PROBE_ID })
  }
  if (attempt.error !== undefined) {
    failed.push(`${facade}.${path} → ${String(attempt.error)}`)
    return
  }

  const produced = hoisted.calls.slice(before)
  if (produced.length === 0) {
    silent.push(`${facade}.${path}`)
    return
  }
  // 记录器的 get/post/put 一律 resolve(null)，异步门面拿到这个替身响应后再做后处理（如
  // impersonationApi.tenants 的 `result.items.map`）必然在 null 上拒绝。这是探针替身造成的，
  // 不是契约缺陷：请求已在同步阶段记下，这里只接住它，免得漏成整轮测试的未处理拒绝。
  // 只接已发出请求的调用——没发请求就拒绝的方法照旧落进 silent，错误也照旧冒出来。
  // 不为此在门面里补 `?.` / `?? []`：那是只为替身存在的静默兜底。
  if (attempt.returned instanceof Promise) {
    attempt.returned.catch(() => {})
  }
  for (const call of produced) {
    endpoints.push({ ...call, facade, path, retried })
  }
}

function walk(facade: string, prefix: string, node: Record<string, unknown>, seen: Set<object>) {
  if (seen.has(node)) {
    return
  }
  seen.add(node)

  for (const [key, value] of Object.entries(node)) {
    const path = prefix ? `${prefix}.${key}` : key
    if (typeof value === 'function') {
      probe(facade, path, value as (...args: unknown[]) => unknown)
    }
    else if (isPlainObject(value) && !isDynamicApiClient(value)) {
      walk(facade, path, value, seen)
    }
  }
}

function describeEndpoint(item: Endpoint) {
  return `${item.facade}.${item.path} → ${item.method} ${item.url}`
}

beforeAll(() => {
  // 排除 *.types.ts：它们不导出门面对象，且 src/modules/chat/api/chat.types.ts 会值转出
  // `~/chat`（链到没有构建产物的 @xihan-ui），直接 glob 会让整份用例文件解析失败。
  const globbed = {
    ...import.meta.glob<Record<string, unknown>>(
      ['/src/api/modules/**/*.ts', '!/src/api/modules/**/*.types.ts'],
      { eager: true },
    ),
    ...import.meta.glob<Record<string, unknown>>(
      ['/src/modules/*/api/**/*.ts', '!/src/modules/*/api/**/*.types.ts'],
      { eager: true },
    ),
  }

  const visitedFacades = new Set<object>()
  for (const moduleExports of Object.values(globbed)) {
    for (const [exportName, exported] of Object.entries(moduleExports)) {
      if (!exportName.endsWith('Api') || !isPlainObject(exported) || visitedFacades.has(exported)) {
        continue
      }
      visitedFacades.add(exported)
      walk(exportName, '', exported, new Set<object>())
    }
  }
})

describe('探针本身的完整性', () => {
  it('每个门面方法都实际发出了请求，没有静默方法', () => {
    expect(silent).toEqual([])
  })

  it('没有任何门面方法在探针调用下抛错——动作名在操作表里查不到时也会落到这里', () => {
    expect(failed).toEqual([])
  })

  it('遍历到的端点数量与门面数量都在合理量级——门面被整体漏掉时这里先失败', () => {
    const facades = new Set(endpoints.map(item => item.facade))

    expect(endpoints.length).toBeGreaterThan(300)
    expect(facades.size).toBeGreaterThan(45)
  })
})

describe('接口地址形状约定', () => {
  it('每条地址都是 /api/<资源>[/{id}][:<动词>] 形态，资源与动词为 kebab-case', () => {
    const bad = endpoints.filter(item => !ROUTE_SHAPE.test(item.url))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('每条请求的「谓词 + 路径模板」都能在操作表里找到——门面拼出的地址就是后端映射的路由', () => {
    const bad = endpoints.filter(item => !knownRoutes.has(`${item.method} ${toTemplate(item.url)}`))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('实例级路由的 {id} 都被填上了，地址里不残留占位符', () => {
    const bad = endpoints.filter(item => item.url.includes('{') || item.url.includes('}'))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('实例段经过 URL 编码，不会把主键里的斜杠或冒号当成路由分隔符', () => {
    const bad = endpoints.filter((item) => {
      const segment = instanceSegment(item.url)
      return segment !== undefined && segment !== encodeURIComponent(decodeURIComponent(segment))
    })

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('地址里没有硬编码 host、协议或端口——baseURL 由 request 层注入', () => {
    const bad = endpoints.filter(item =>
      item.url.includes('://') || item.url.startsWith('//') || /localhost|127\.0\.0\.1/i.test(item.url))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('地址里不带查询串——查询参数一律走 params 而不是拼进路径', () => {
    const bad = endpoints.filter(item => item.url.includes('?') || item.url.includes('&'))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('动词不以 async 结尾——后端方法名的 Async 后缀不进路由', () => {
    const bad = endpoints.filter(item => customVerb(item.url)?.endsWith('-async'))

    expect(bad.map(describeEndpoint)).toEqual([])
  })
})

describe('方法命名与操作动词的一致性', () => {
  it('名为 detail 或以 get 开头的读取方法落到 get 动词族：GET 路由或 :get-… 自定义动词', () => {
    const bad = endpoints.filter((item) => {
      const name = lastName(item.path)
      if (name !== 'detail' && !/^get[A-Z]/.test(name)) {
        return false
      }
      return item.method !== 'GET' && !customVerb(item.url)?.startsWith('get')
    })

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('名为 create 的方法落到标准动词 create：集合级 POST /api/<资源>', () => {
    const bad = endpoints.filter(item => lastName(item.path) === 'create'
      && (item.method !== 'POST' || instanceSegment(item.url) !== undefined || customVerb(item.url) !== undefined))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('名为 update 的方法落到实例级的 update 动词族：标准动词 PUT，或挂在父资源上的 :update-… 自定义动词', () => {
    // 角色授权、租户成员这类子资源的更新由后端挂在父资源上（role/{id}:update-permission），
    // 它们是自定义动词所以走 POST；这里只要求落在实例级路由且动词属于 update 族。
    const updates = endpoints.filter(item => lastName(item.path) === 'update')
    const bad = updates.filter((item) => {
      if (instanceSegment(item.url) === undefined) {
        return true
      }
      const verb = customVerb(item.url)
      return verb === undefined ? item.method !== 'PUT' : !verb.startsWith('update-')
    })

    expect(updates.length).toBeGreaterThan(40)
    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('名为 delete 的方法落到标准动词 delete：DELETE 实例级路由', () => {
    const bad = endpoints.filter(item => lastName(item.path) === 'delete'
      && (item.method !== 'DELETE' || instanceSegment(item.url) === undefined))

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('分页方法落到 list 动词族：扁平请求的 GET /api/<资源>，或 POST /api/<资源>:list…', () => {
    const bad = endpoints.filter((item) => {
      if (!isPageLike(lastName(item.path))) {
        return false
      }
      const verb = customVerb(item.url)
      return verb === undefined ? item.method !== 'GET' : !(item.method === 'POST' && verb.startsWith('list'))
    })

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('标准动词只用 GET / POST / PUT / DELETE 的本义——自定义动词一律 POST', () => {
    const bad = endpoints.filter(item => customVerb(item.url) !== undefined && item.method !== 'POST')

    expect(bad.map(describeEndpoint)).toEqual([])
  })
})

describe('参数位置约定', () => {
  it('读取与删除谓词不带请求体——后端 GET / DELETE 只做路由值与查询串绑定', () => {
    const bad = endpoints.filter(item => (item.method === 'GET' || item.method === 'DELETE')
      && item.body !== undefined)

    expect(bad.map(item => `${item.facade}.${item.path}`)).toEqual([])
  })

  it('分页查询对象原样作 body 上送，不被拆成查询串', () => {
    const pageCalls = endpoints.filter(item => isPageLike(lastName(item.path))
      && item.url !== READONLY_RUNTIME_PAGE)
    const bad = pageCalls.filter(item => item.body !== PAGE_QUERY || item.params !== undefined)

    expect(pageCalls.length).toBeGreaterThan(40)
    expect(bad.map(item => `${item.facade}.${item.path}`)).toEqual([])
  })

  it('除零代码运行时只读分页外，分页一律走 POST', () => {
    const bad = endpoints.filter(item => isPageLike(lastName(item.path))
      && item.method !== 'POST'
      && item.url !== READONLY_RUNTIME_PAGE)

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('唯一走 GET 的分页只有零代码运行时——多出第二个就说明有人绕开了统一约定', () => {
    const getPages = endpoints.filter(item => isPageLike(lastName(item.path)) && item.method === 'GET')

    expect(getPages.map(describeEndpoint)).toEqual([`codeGenRuntimeApi.page → GET ${READONLY_RUNTIME_PAGE}`])
  })

  it('门面自己把 id 放进请求的调用一律落到实例级路由，且路径段就是这个 id', () => {
    const bad = endpoints.filter((item) => {
      if (item.retried) {
        return false
      }
      const source = [item.body, item.params].find(value => isPlainObject(value) && value.id !== undefined && value.id !== null && value.id !== '')
      if (!isPlainObject(source)) {
        return false
      }
      return instanceSegment(item.url) !== encodeURIComponent(String(source.id))
    })

    expect(bad.map(describeEndpoint)).toEqual([])
  })

  it('查询参数的键名不含空格与斜杠，能安全拼进查询串', () => {
    const bad: string[] = []
    for (const item of endpoints) {
      if (!isPlainObject(item.params)) {
        continue
      }
      for (const key of Object.keys(item.params)) {
        if (key.length === 0 || /[\s/?&=#]/.test(key)) {
          bad.push(`${item.facade}.${item.path} → ${key}`)
        }
      }
    }

    expect(bad).toEqual([])
  })
})

describe('门面内部的路由唯一性', () => {
  it('同一个门面对象内不出现重复的「谓词 + URL」组合——重复即意味着某个方法拼错了动作名', () => {
    const duplicates: string[] = []
    const byFacade = new Map<string, Map<string, string>>()

    for (const item of endpoints) {
      const table = byFacade.get(item.facade) ?? new Map<string, string>()
      byFacade.set(item.facade, table)
      const key = `${item.method} ${item.url}`
      const owner = table.get(key)
      if (owner && owner !== item.path) {
        duplicates.push(`${item.facade}: ${owner} 与 ${item.path} 同为 ${key}`)
      }
      else {
        table.set(key, item.path)
      }
    }

    expect(duplicates).toEqual(KNOWN_DUPLICATE_ROUTES)
  })
})

describe('资源命名约定', () => {
  it('只读资源（-query 结尾）不出现 PUT / DELETE', () => {
    const resources = new Map<string, Set<string>>()
    for (const item of endpoints) {
      const resource = /^\/api\/([^/:]+)/.exec(item.url)?.[1] ?? ''
      const methods = resources.get(resource) ?? new Set<string>()
      methods.add(item.method)
      resources.set(resource, methods)
    }

    const bad = [...resources.entries()]
      .filter(([name, methods]) => name.endsWith('-query') && (methods.has('DELETE') || methods.has('PUT')))
      .map(([name]) => name)

    expect(bad).toEqual([])
  })

  it('每个资源名都不以 controller / service / app-service 结尾', () => {
    const bad = [...new Set(endpoints.map(item => /^\/api\/([^/:]+)/.exec(item.url)?.[1] ?? ''))]
      .filter(name => /-(?:controller|service)$/.test(name))

    expect(bad).toEqual([])
  })
})
