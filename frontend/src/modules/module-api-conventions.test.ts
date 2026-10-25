/**
 * 业务模块 API 客户端的 5.0 路由约定回归锚点（src/modules/<模块>/api）。
 *
 * 职责边界：模块侧的动作名普遍是短名（Page / Detail / Create），由 operations.generated.ts
 * 解析到 5.0 的资源与动词。这里为「落到哪个资源与动词、主键是否进路径、参数走查询串还是请求体」
 * 各配一条用例。不测 .vue，也不测导入了 @xihan-ui 的模块。
 */
import type { AxiosRequestConfig } from '~/request'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { aiAssistantApi } from './ai/api/assistant'
import { chatAssistantProviderApi } from './ai/api/chat-assistant'
import { knowledgeApi } from './ai/api/knowledge'
import { aiProviderApi } from './ai/api/provider'
import { chatApi } from './chat/api/chat'
import { chatAuditApi } from './chat/api/chat-audit'
import { codeGenTableColumnApi } from './codegen/api/codegen-table-column'
import { codeGenRuntimeApi } from './codegen/api/dynamic-runtime'
import { codeGenerationApi } from './codegen/api/generation'
import { printDataSourceApi } from './printing/api/print-data-source'
import { printTemplateApi } from './printing/api/print-template'
import { workflowDefinitionApi } from './workflow/api/definition'
import { workflowTodoApi } from './workflow/api/todo'

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

const { calls } = hoisted

beforeEach(() => {
  calls.length = 0
})

function only(): RecordedCall {
  expect(calls).toHaveLength(1)
  const call = calls[0]
  if (!call) {
    throw new Error('没有记录到任何请求调用')
  }
  return call
}

describe('零代码只读运行时', () => {
  it('取表结构是 POST 自定义动词，读调用的 tableId 改随请求体上送', async () => {
    await codeGenRuntimeApi.getSchema('101')

    expect(only()).toEqual({ method: 'POST', url: '/api/dynamic-runtime:get-schema', body: { tableId: '101' }, params: undefined })
  })

  it('运行时分页是扁平请求，推导为 GET /api/dynamic-runtime + 查询串，且空值被裁掉', async () => {
    await codeGenRuntimeApi.page({ tableId: '101', pageIndex: 1, pageSize: 20 })

    expect(only()).toEqual({
      method: 'GET',
      url: '/api/dynamic-runtime',
      params: { TableId: '101', PageIndex: 1, PageSize: 20 },
    })
  })

  it('运行时分页的页码 0 是有效取值不会被裁掉——只有 undefined/null/空串才滤', async () => {
    await codeGenRuntimeApi.page({ tableId: '101', pageIndex: 0, pageSize: 20 })

    expect(only().params).toEqual({ TableId: '101', PageIndex: 0, PageSize: 20 })
  })
})

describe('打印模板的参数与作用域', () => {
  it('详情是实例级自定义动词：主键进路径，主键与作用域随请求体上送', async () => {
    await printTemplateApi.detail('1975', 'Tenant' as never)

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/print-template-query/1975:get-print-template-detail',
      body: { id: '1975', scope: 'Tenant' },
      params: undefined,
    })
  })

  it('删除落到 DELETE /api/print-template/{id}，DTO 显式展开成三个查询参数——DELETE 不发请求体', async () => {
    await printTemplateApi.delete({ id: '1', concurrencyStamp: 'v2', scope: 'Tenant' } as never)

    expect(only()).toEqual({
      method: 'DELETE',
      url: '/api/print-template/1',
      params: { id: '1', concurrencyStamp: 'v2', scope: 'Tenant' },
    })
  })

  it('按编码解析模板是集合级自定义动词，编码与作用域都在请求体', async () => {
    await printTemplateApi.resolveByCode('sales-order', 'Auto' as never)

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/print-template-query:get-resolved-print-template-by-code',
      body: { scope: 'Auto', templateCode: 'sales-order' },
      params: undefined,
    })
  })

  it('数据源目录只有一个只读入口，走 GET /api/print-data-source-query', async () => {
    await printDataSourceApi.list()

    expect(only()).toEqual({ method: 'GET', url: '/api/print-data-source-query', params: undefined })
  })
})

describe('聊天接口的动词与参数位置', () => {
  it('建群走 POST /api/chat:create-group-conversation，参数在请求体', async () => {
    await chatApi.createGroupConversation('研发群', ['1', '2'])

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/chat:create-group-conversation',
      body: { conversationName: '研发群', memberUserIds: ['1', '2'] },
      params: undefined,
    })
  })

  it('加成员与移除成员都是集合级自定义动词，移除不再降级为 DELETE + 查询串', async () => {
    await chatApi.addMembers('c1', ['9'])
    await chatApi.removeMember('c1', '9')

    expect(calls).toEqual([
      { method: 'POST', url: '/api/chat:create-members', body: { conversationId: 'c1', userIds: ['9'] }, params: undefined },
      { method: 'POST', url: '/api/chat:delete-member', body: { conversationId: 'c1', userId: '9' }, params: undefined },
    ])
  })

  it('主动退群走独立端点，且请求体不带 userId', async () => {
    // 回归锚点：界面上的「退出群聊」原本调 removeMember(会话, 自己)，而那个端点挂 chat.manage，
    // 普通成员在权限过滤器阶段就被 403 —— 按钮点了没反应，错误还被组件的 catch 吞掉。
    // 退出对象必须由服务端从登录态解析：请求体一旦带 userId，就等于开了「改包退别人的群」的口子。
    await chatApi.leaveConversation('c1')

    expect(calls).toEqual([
      { method: 'POST', url: '/api/chat:leave-conversation', body: { conversationId: 'c1' }, params: undefined },
    ])
    expect(calls[0]?.body).not.toHaveProperty('userId')
  })

  it('编辑消息与改会话信息是自定义动词，走 POST 而不是 PUT', async () => {
    await chatApi.editMessage('m1', '新内容')
    await chatApi.updateConversationInfo({ conversationId: 'c1' } as never)

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/chat:edit-message',
      'POST /api/chat:update-conversation-info',
    ])
    expect(calls[0]?.body).toEqual({ messageId: 'm1', content: '新内容' })
  })

  it('撤回消息的 messageId 走查询串，请求体为空', async () => {
    await chatApi.recallMessage('m9')

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/chat:recall-message',
      body: undefined,
      params: { messageId: 'm9' },
    })
  })

  it('单聊、发消息、已读与置顶都是集合级自定义动词', async () => {
    await chatApi.openSingleConversation('u1')
    await chatApi.sendMessage({ conversationId: 'c1' } as never)
    await chatApi.markRead('c1')
    await chatApi.pinMessage('m1')

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/chat:open-single-conversation',
      'POST /api/chat:send-message',
      'POST /api/chat:mark-read',
      'POST /api/chat:pin-message',
    ])
  })

  it('轻量选人默认取 20 条，关键字为空时不下发 Keyword', async () => {
    await chatApi.userOptions('')
    await chatApi.userOptions('张', 5)

    expect(calls.map(item => [item.url, item.body])).toEqual([
      ['/api/chat-query:get-user-options', { Limit: 20 }],
      ['/api/chat-query:get-user-options', { Limit: 5, Keyword: '张' }],
    ])
  })

  it('会话已读位置、置顶消息与我的会话走独立的只读资源 chat-query', async () => {
    await chatApi.readPositions('c1')
    await chatApi.pinnedMessages('c1')
    await chatApi.myConversations()

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/chat-query:get-read-positions',
      'POST /api/chat-query:get-pinned-messages',
      'POST /api/chat-query:get-my-conversations',
    ])
  })

  it('聊天审计分页走管理侧的 chat-audit-query', async () => {
    await chatAuditApi.page({ conditions: { filters: [], keyword: null, sorts: [] }, page: { pageIndex: 1, pageSize: 20 } } as never)

    expect(only()).toMatchObject({ method: 'POST', url: '/api/chat-audit-query:list-chat-message' })
  })
})

describe('智能助手模块的读写资源', () => {
  it('助手管理的读写分处 ai-assistant 与 ai-assistant-query 两个资源', async () => {
    await aiAssistantApi.create({} as never)
    await aiAssistantApi.updateStatus({ id: '7' } as never)
    await aiAssistantApi.delete('7')
    await aiAssistantApi.page({} as never)
    await aiAssistantApi.detail('7')

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/ai-assistant',
      'POST /api/ai-assistant/7:update-status',
      'DELETE /api/ai-assistant/7',
      'POST /api/ai-assistant-query:list',
      'POST /api/ai-assistant-query/7:get-detail',
    ])
  })

  it('设为默认把主键放进路径并包成 { id } 上送，删除则把主键放路径和查询串', async () => {
    await aiAssistantApi.setDefault('7')
    await aiAssistantApi.delete('7')

    expect(calls).toEqual([
      { method: 'POST', url: '/api/ai-assistant/7:set-default', body: { id: '7' }, params: undefined },
      { method: 'DELETE', url: '/api/ai-assistant/7', params: { id: '7' } },
    ])
  })

  it('供应商连通性测试与设为默认同样是实例级动词，用 { id } 请求体', async () => {
    await aiProviderApi.testConnection('3')

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/ai-provider/3:test-connection',
      body: { id: '3' },
      params: undefined,
    })
  })

  it('知识库落在三个资源：写入 knowledge-document、读取 knowledge-document-query、检索 knowledge-query', async () => {
    await knowledgeApi.ingest({} as never)
    await knowledgeApi.detail('1')
    await knowledgeApi.query({} as never)

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/knowledge-document:ingest',
      'POST /api/knowledge-document-query/1:get-detail',
      'GET /api/knowledge-query',
    ])
  })

  it('聊天助手的可用列表挂在 ai-assistant-query:get-available（只要登录态，不看助手管理权限）', async () => {
    await chatAssistantProviderApi.availableAssistants()
    await chatAssistantProviderApi.openConversation('a1')
    await chatAssistantProviderApi.reply('c1', 'r1')

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/ai-assistant-query:get-available',
      'POST /api/chat-assistant:open-conversation',
      'POST /api/chat-assistant:reply',
    ])
    expect(calls[1]?.body).toEqual({ assistantId: 'a1' })
    expect(calls[2]?.body).toEqual({ conversationId: 'c1', replyId: 'r1' })
  })
})

describe('代码生成与工作流的动词', () => {
  it('代码生成编排的动作都是集合级自定义动词', async () => {
    await codeGenerationApi.importTable({} as never)
    await codeGenerationApi.preview({} as never)
    await codeGenerationApi.generate({} as never)
    await codeGenerationApi.syncSchema('5')

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/code-generation:import-table',
      'POST /api/code-generation:preview',
      'POST /api/code-generation:generate',
      'POST /api/code-generation:sync-schema',
    ])
    expect(calls[3]?.body).toEqual({ tableId: '5' })
  })

  it('列可导入数据库表是 POST 自定义动词，读调用的参数改随请求体上送并裁掉空关键字', async () => {
    await codeGenerationApi.listDatabaseTables({ dataSourceId: '1', keyword: '' })

    expect(only()).toEqual({
      method: 'POST',
      url: '/api/code-generation:list-database-tables',
      body: { DataSourceId: '1' },
      params: undefined,
    })
  })

  it('列配置批量保存走集合级 POST，单条更新走 PUT /{id}，按表查询走 POST 自定义动词', async () => {
    await codeGenTableColumnApi.batchSave({} as never)
    await codeGenTableColumnApi.update({ id: '4' } as never)
    await codeGenTableColumnApi.getByTable('9')

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/code-gen-table-column:batch-save',
      'PUT /api/code-gen-table-column/4',
      'POST /api/code-gen-table-column-query:get-by-table',
    ])
    expect(calls[2]?.body).toEqual({ tableId: '9' })
  })

  it('流程定义的草稿更新与状态流转都是实例级自定义动词，只有新版本是集合级', async () => {
    await workflowDefinitionApi.updateDraft({ id: '1' } as never)
    await workflowDefinitionApi.publish({ id: '1' } as never)
    await workflowDefinitionApi.newVersion({ id: '1' } as never)
    await workflowDefinitionApi.disable({ id: '1' } as never)
    await workflowDefinitionApi.archive({ id: '1' } as never)

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/workflow-definition/1:update-draft',
      'POST /api/workflow-definition/1:publish',
      'POST /api/workflow-definition:new-version',
      'POST /api/workflow-definition/1:disable',
      'POST /api/workflow-definition/1:archive',
    ])
  })

  it('待办只有分页一个读入口，受理人由服务端锁定所以不带用户参数', async () => {
    const query = { conditions: { filters: [], keyword: null, sorts: [] }, page: { pageIndex: 1, pageSize: 20 } }
    await workflowTodoApi.page(query as never)

    expect(only()).toEqual({ method: 'POST', url: '/api/workflow-todo-query:list', body: query, params: undefined })
  })

  it('待办的三个写动作都是集合级自定义动词，加签的 Add 映射为 create', async () => {
    await workflowTodoApi.complete({} as never)
    await workflowTodoApi.transfer({} as never)
    await workflowTodoApi.addAssignees({} as never)

    expect(calls.map(item => `${item.method} ${item.url}`)).toEqual([
      'POST /api/workflow-todo:complete',
      'POST /api/workflow-todo:transfer',
      'POST /api/workflow-todo:create-assignees',
    ])
  })
})
