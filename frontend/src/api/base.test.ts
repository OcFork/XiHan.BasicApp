/**
 * Dynamic API 底座（src/api/base.ts）单元测试。
 *
 * 职责边界：facade 的「控制器/动作」经路由表解析成 5.0 操作路由后，HTTP 方法、URL（含实例级 {id}）、
 * 请求体与查询串各放什么；以及 appendDynamicApiParam / formatDynamicApiRouteValue 两个纯函数的全分支行为。
 * 路由表被替换成下面的固定样本，不依赖后端源码；不发真实请求：requestClient 被整体替换为记录器。
 */
import type { DynamicApiParams } from './base'
import type { AxiosRequestConfig } from '~/request'
import { describe, expect, it, vi } from 'vitest'
import {
  appendDynamicApiParam,
  createCommandApi,
  createDynamicApiClient,
  createReadApi,
  formatDynamicApiRouteValue,
  resolveOperationRoute,
} from './base'

interface RecordedCall {
  method: 'delete' | 'get' | 'post' | 'put'
  url: string
  body?: unknown
  config?: AxiosRequestConfig & { params?: unknown }
}

const hoisted = vi.hoisted(() => ({ calls: [] as RecordedCall[] }))

vi.mock('./operations.generated', () => ({
  OPERATIONS: {
    // 同名动作对应三个操作：按谓词区分
    'Order/Order': {
      DELETE: { id: 'order.delete', method: 'DELETE', path: '/api/order/{id}' },
      POST: { id: 'order.create', method: 'POST', path: '/api/order' },
      PUT: { id: 'order.update', method: 'PUT', path: '/api/order/{id}' },
    },
    // facade 写 POST、方法名前缀推出 GET：唯一候选时谓词不参与判断
    'OrderQuery/OrderPage': { GET: { id: 'order-query.list-order', method: 'POST', path: '/api/order-query:list-order' } },
    'OrderQuery/OrderDetail': { GET: { id: 'order-query.get-order-detail', method: 'POST', path: '/api/order-query/{id}:get-order-detail' } },
    'OrderQuery/Options': { GET: { id: 'order-query.list', method: 'GET', path: '/api/order-query' } },
    'Order/Cancel': { POST: { id: 'order.cancel', method: 'POST', path: '/api/order/{id}:cancel' } },
    'Order/Archive': { DELETE: { id: 'order.archive', method: 'DELETE', path: '/api/order/{id}' } },
    'Order/Purge': { POST: { id: 'order.purge', method: 'DELETE', path: '/api/order/{id}' } },
    'Order/Export': { POST: { id: 'order.export', method: 'GET', path: '/api/order' } },
    'Order/Sync': {
      DELETE: { id: 'order.sync', method: 'POST', path: '/api/order:sync' },
      POST: { id: 'order.sync-2', method: 'POST', path: '/api/order:sync-2' },
    },
  },
}))

vi.mock('@/api/request', () => ({
  requestClient: {
    get(url: string, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'get', url, config })
      return Promise.resolve('get-result')
    },
    post(url: string, body?: unknown, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'post', url, body, config })
      return Promise.resolve('post-result')
    },
    put(url: string, body?: unknown, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'put', url, body, config })
      return Promise.resolve('put-result')
    },
    delete(url: string, config?: AxiosRequestConfig) {
      hoisted.calls.push({ method: 'delete', url, config })
      return Promise.resolve('delete-result')
    },
  },
}))

const { calls } = hoisted

function lastCall(): RecordedCall {
  const call = calls.at(-1)
  if (!call) {
    throw new Error('没有记录到任何请求调用')
  }
  return call
}

describe('resolveOperationRoute 的操作解析', () => {
  it('动作只对应一个操作时直接用它，不看 facade 写的谓词', () => {
    expect(resolveOperationRoute('POST', 'OrderQuery', 'OrderPage').id).toBe('order-query.list-order')
    expect(resolveOperationRoute('GET', 'OrderQuery', 'OrderPage').id).toBe('order-query.list-order')
  })

  it('同名动作对应多个操作时按谓词挑选', () => {
    expect(resolveOperationRoute('POST', 'Order', 'Order').id).toBe('order.create')
    expect(resolveOperationRoute('PUT', 'Order', 'Order').id).toBe('order.update')
    expect(resolveOperationRoute('DELETE', 'Order', 'Order').id).toBe('order.delete')
  })

  it('多个候选里没有该谓词时抛错并列出候选，而不是随便挑一个', () => {
    expect(() => resolveOperationRoute('GET', 'Order', 'Sync')).toThrow(/order\.sync、order\.sync-2/)
  })

  it('路由表里没有的动作直接抛错——对不上就是 404，不能静默拼出旧式 URL', () => {
    expect(() => resolveOperationRoute('GET', 'Order', 'Missing')).toThrow(/没有对应的 5\.0 操作/)
  })

  it('控制器名与动作名两端的空白与斜杠被剥离后再查表', () => {
    expect(resolveOperationRoute('POST', ' /Order/ ', '/Cancel/').id).toBe('order.cancel')
  })

  it('空控制器名在建客户端时立刻抛错，空动作名在调用时抛错', () => {
    expect(() => createDynamicApiClient('  ')).toThrow('Dynamic API 路由段不能为空')
    expect(() => createDynamicApiClient('Order').get('')).toThrow('Dynamic API 路由段不能为空')
  })
})

describe('读取类调用（facade 的 GET / DELETE）', () => {
  it('落到 POST 路由时参数表作 JSON 请求体，保留布尔与数字类型，不进查询串', async () => {
    await createDynamicApiClient('OrderQuery').get('OrderPage', { Keyword: '张', Limit: 20, OnlyEnabled: false })
    expect(lastCall()).toEqual({ method: 'post', url: '/api/order-query:list-order', body: { Keyword: '张', Limit: 20, OnlyEnabled: false }, config: {} })
  })

  it('落到实例级路由时 id 进路径，同时留在请求体里', async () => {
    await createDynamicApiClient('OrderQuery').get('OrderDetail', { id: '1975' })
    expect(lastCall()).toMatchObject({ method: 'post', url: '/api/order-query/1975:get-order-detail', body: { id: '1975' } })
  })

  it('落到 GET 路由时参数走查询串，没有请求体', async () => {
    await createDynamicApiClient('OrderQuery').get('Options', { Keyword: '张' })
    expect(lastCall()).toEqual({ method: 'get', url: '/api/order-query', config: { params: { Keyword: '张' } } })
  })

  it('落到 DELETE 路由时 id 进路径、其余参数走查询串', async () => {
    await createDynamicApiClient('Order').delete('Archive', { id: '7', reason: '重复' })
    expect(lastCall()).toEqual({ method: 'delete', url: '/api/order/7', config: { params: { id: '7', reason: '重复' } } })
  })

  it('params 位与 config.params 按键合并，params 位覆盖同名键；params 位没传时 config.params 不被清掉', async () => {
    await createDynamicApiClient('OrderQuery').get('Options', { A: 'params' }, { params: { A: 'config', B: 'config' } })
    expect(lastCall().config?.params).toEqual({ A: 'params', B: 'config' })
    await createDynamicApiClient('OrderQuery').get('Options', undefined, { params: { B: 'config' } })
    expect(lastCall().config?.params).toEqual({ B: 'config' })
  })

  it('实例级路由缺 id 时在调用前就抛错', () => {
    expect(() => createDynamicApiClient('Order').delete('Archive', { reason: '重复' })).toThrow(/实例级路由/)
  })

  it('主键里的斜杠被编码，不会把一个值拆成多个路由段', async () => {
    await createDynamicApiClient('Order').delete('Archive', { id: 'a/b' })
    expect(lastCall().url).toBe('/api/order/a%2Fb')
  })
})

describe('写入类调用（facade 的 POST / PUT）', () => {
  it('请求对象整体作 JSON 请求体，config 原样透传', async () => {
    const onUploadProgress = vi.fn()
    await createDynamicApiClient('Order').post('Order', { name: '订单' }, { onUploadProgress })
    expect(lastCall()).toEqual({ method: 'post', url: '/api/order', body: { name: '订单' }, config: { onUploadProgress } })
  })

  it('实例级路由的 id 取自请求体', async () => {
    await createDynamicApiClient('Order').put('Order', { id: '3', name: '改名' })
    expect(lastCall()).toEqual({ method: 'put', url: '/api/order/3', body: { id: '3', name: '改名' }, config: {} })
  })

  it('请求体没有 id 时取 config.params 里的 id，config.params 照常走查询串', async () => {
    await createDynamicApiClient('Order').post('Cancel', undefined, { params: { id: '9' } })
    expect(lastCall()).toEqual({ method: 'post', url: '/api/order/9:cancel', body: undefined, config: { params: { id: '9' } } })
  })

  it('落到 DELETE / GET 路由（不读请求体）时请求对象的标量字段并进查询串', async () => {
    await createDynamicApiClient('Order').post('Purge', { id: '5', physical: true })
    expect(lastCall()).toEqual({ method: 'delete', url: '/api/order/5', config: { params: { id: '5', physical: true } } })
    await createDynamicApiClient('Order').post('Export', { format: 'xlsx', skipped: null })
    expect(lastCall()).toEqual({ method: 'get', url: '/api/order', config: { params: { format: 'xlsx' } } })
  })

  it('落到 DELETE / GET 路由时请求对象里的嵌套对象进不了查询串，直接报错而不是静默丢字段', () => {
    expect(() => createDynamicApiClient('Order').post('Export', { filter: { a: 1 } })).toThrow(/字段 filter 是对象/)
  })

  it('四个谓词把 requestClient 的返回值原样透出，不做二次包装', async () => {
    const api = createDynamicApiClient('Order')
    await expect(api.post('Order', {})).resolves.toBe('post-result')
    await expect(api.put('Order', { id: '1' })).resolves.toBe('put-result')
    await expect(api.delete('Order', { id: '1' })).resolves.toBe('delete-result')
    await expect(createDynamicApiClient('OrderQuery').get('Options')).resolves.toBe('get-result')
  })
})

describe('createReadApi / createCommandApi 的约定', () => {
  it('分页的动作名为「资源段 + Page」，整个查询对象作请求体', async () => {
    const query = { conditions: { filters: [], keyword: null, sorts: [] }, page: { pageIndex: 1, pageSize: 20 } }
    await createReadApi('OrderQuery', 'Order').page(query)
    expect(lastCall()).toMatchObject({ method: 'post', url: '/api/order-query:list-order', body: query })
  })

  it('详情的动作名为「资源段 + Detail」，主键进实例级路由', async () => {
    await createReadApi('OrderQuery', '/Order/').detail('1024')
    expect(lastCall()).toMatchObject({ method: 'post', url: '/api/order-query/1024:get-order-detail', body: { id: '1024' } })
  })

  it('空资源段建读 API 时立刻抛错', () => {
    expect(() => createReadApi('OrderQuery', ' ')).toThrow('Dynamic API 路由段不能为空')
  })

  it('新增走 POST、更新走 PUT，两者共用不带动词的资源段动作，由谓词区分操作', async () => {
    const api = createCommandApi<unknown, { id: string }, unknown>('Order', 'Order')
    await api.create({ name: 'a' })
    await api.update({ id: '2' })
    expect(calls.slice(-2).map(call => `${call.method} ${call.url}`)).toEqual(['post /api/order', 'put /api/order/2'])
  })
})

describe('appendDynamicApiParam 的空值过滤', () => {
  it('undefined / null / 空串一律不写入参数表，避免拼出 ?Keyword= 这种空条件', () => {
    const params: DynamicApiParams = {}
    appendDynamicApiParam(params, 'A', undefined)
    appendDynamicApiParam(params, 'B', null)
    appendDynamicApiParam(params, 'C', '')

    expect(params).toEqual({})
  })

  it('0 / false / 负数 / 纯空格字符串是有效取值，必须写入', () => {
    const params: DynamicApiParams = {}
    appendDynamicApiParam(params, 'Zero', 0)
    appendDynamicApiParam(params, 'False', false)
    appendDynamicApiParam(params, 'Negative', -1)
    appendDynamicApiParam(params, 'Blank', ' ')

    expect(params).toEqual({ Zero: 0, False: false, Negative: -1, Blank: ' ' })
  })

  it('同名键后写覆盖先写，并且不因为写入 undefined 而被删除（undefined 直接跳过）', () => {
    const params: DynamicApiParams = { Keyword: '旧值' }
    appendDynamicApiParam(params, 'Keyword', '新值')
    expect(params.Keyword).toBe('新值')

    appendDynamicApiParam(params, 'Keyword', undefined)
    expect(params.Keyword).toBe('新值')
  })

  it('中文、emoji 与超长字符串原样写入，不截断也不转码', () => {
    const long = '甲'.repeat(5000)
    const params: DynamicApiParams = {}
    appendDynamicApiParam(params, 'Keyword', '张三🙂')
    appendDynamicApiParam(params, 'Long', long)

    expect(params.Keyword).toBe('张三🙂')
    expect(params.Long).toHaveLength(5000)
  })
})

describe('formatDynamicApiRouteValue 的路由段编码', () => {
  it('普通雪花 ID 原样返回', () => {
    expect(formatDynamicApiRouteValue('1975062400000001')).toBe('1975062400000001')
  })

  it('斜杠与井号被编码，避免把一个值拆成多个路由段', () => {
    expect(formatDynamicApiRouteValue('a/b')).toBe('a%2Fb')
    expect(formatDynamicApiRouteValue('a#b')).toBe('a%23b')
    expect(formatDynamicApiRouteValue('a?b=c&d')).toBe('a%3Fb%3Dc%26d')
  })

  it('中文与 emoji 走 UTF-8 百分号编码', () => {
    expect(formatDynamicApiRouteValue('模板')).toBe('%E6%A8%A1%E6%9D%BF')
    expect(formatDynamicApiRouteValue('🙂')).toBe('%F0%9F%99%82')
  })

  it('空串返回空串——调用方需自行保证主键非空', () => {
    expect(formatDynamicApiRouteValue('')).toBe('')
  })
})
