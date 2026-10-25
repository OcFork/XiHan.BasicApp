// ----------------------------------------------------------------
// 操作路由表生成器（零依赖）：
//   node scripts/generate-operations.mjs            生成 src/api/operations.generated.ts
//   node scripts/generate-operations.mjs --check    只比对，文件过期时以非 0 退出（CI 门禁）
//   node scripts/generate-operations.mjs --verify <openapi.json 路径或 URL>
//                                                    额外核对推导结果与宿主 OpenAPI 完全一致
//
// 前端 API facade 仍按 4.x 动态 API 的「控制器/动作」书写（createDynamicApiClient('UserQuery').post('UserPage')），
// 后端 5.0 的接口是操作（user-query.list-user → POST /api/user-query:list-user）。本脚本从后端源码推出两边的对应：
//   1. 扫 backend/src/modules/*/Operations/**/*Operation.cs：[Operation("<id>")]、注入的服务类型、调用的服务方法、请求类型；
//   2. 「控制器/动作」按 4.x DefaultDynamicApiConvention：服务名去 I 前缀与 AppService/Service 后缀，方法名去 Async 与动词前缀；
//   3. 路由按 5.0 HttpRouteConvention（ADR-0015）：请求类型（含基类）声明了 Id 属性的走实例级 /{id}，否则落在集合上；
//   4. 非操作的 ASP.NET 端点（文件上传 / 下载）在 EXTRA_ENDPOINTS 里登记。
// ----------------------------------------------------------------
import { existsSync, readdirSync, readFileSync, writeFileSync } from 'node:fs'
import { dirname, join, relative } from 'node:path'
import process from 'node:process'
import { fileURLToPath } from 'node:url'

const FRONTEND = join(dirname(fileURLToPath(import.meta.url)), '..')
const BACKEND_SRC = join(FRONTEND, '..', 'backend', 'src')
const OUTPUT = join(FRONTEND, 'src', 'api', 'operations.generated.ts')
/** 与宿主 XiHan:Protocols:Http:RoutePrefix 一致 */
const ROUTE_PREFIX = '/api'

/** 4.x DynamicApiConventionOptions.HttpMethodConventions 的默认表：方法名前缀 → 谓词 */
const HTTP_PREFIXES = Object.fromEntries([
  ['GET', ['Get', 'Retrieve', 'Fetch', 'Find', 'Query', 'List', 'Search']],
  ['POST', ['Create', 'Add', 'Insert']],
  ['PUT', ['Update', 'Edit', 'Modify']],
  ['DELETE', ['Delete', 'Remove', 'Destroy']],
  ['PATCH', ['Patch', 'PartialUpdate']],
].flatMap(([verb, prefixes]) => prefixes.map(prefix => [prefix, verb])))
const PREFIXES_BY_LENGTH = Object.keys(HTTP_PREFIXES).sort((a, b) => b.length - a.length)
const SERVICE_SUFFIXES = ['ApplicationService', 'AppService', 'Service']

/**
 * 不是操作的 ASP.NET 端点：操作模型只收 JSON，上传（multipart）与下载（流）由后端 FileTransferEndpoints 提供。
 * 键与操作一样是「控制器/动作」，谓词是前端调用时用的 4.x 谓词。
 */
const EXTRA_ENDPOINTS = [
  { key: 'File/UploadFile', verb: 'POST', route: { id: 'file.upload', method: 'POST', path: `${ROUTE_PREFIX}/file:upload` } },
  { key: 'File/DownloadFile', verb: 'POST', route: { id: 'file.download', method: 'POST', path: `${ROUTE_PREFIX}/file:download` } },
]

const args = process.argv.slice(2)
const checkOnly = args.includes('--check')
const verifyIndex = args.indexOf('--verify')
const verifySource = verifyIndex >= 0 ? args[verifyIndex + 1] : undefined

function listFiles(dir, predicate, out = []) {
  if (!existsSync(dir))
    return out
  for (const entry of readdirSync(dir, { withFileTypes: true })) {
    if (entry.name === 'bin' || entry.name === 'obj')
      continue
    const full = join(dir, entry.name)
    if (entry.isDirectory())
      listFiles(full, predicate, out)
    else if (predicate(entry.name))
      out.push(full)
  }
  return out
}

function readSource(file) {
  return readFileSync(file, 'utf8').replace(/^\uFEFF/, '')
}

/** 从 index 开始取一个配对的括号段（含两端），找不到返回空串 */
function balanced(text, index, open, close) {
  if (text[index] !== open)
    return ''
  let depth = 0
  for (let i = index; i < text.length; i++) {
    if (text[i] === open)
      depth++
    else if (text[i] === close && --depth === 0)
      return text.slice(index, i + 1)
  }
  return ''
}

/**
 * 类型索引：类型名 → [{ parameters, body, bases }]（同名类型可能出现在多个命名空间，任一满足即可）。
 * 只做判断「是否声明了 Id 属性」所需的最小语法：主构造 / 记录位置参数、类体、基类列表。
 */
function indexTypes(files) {
  const types = new Map()
  const declaration = /\b(?:class|record|struct)\s+(\w+)(?:<[^>{(]*>)?\s*/g
  for (const file of files) {
    const text = readSource(file)
    for (const match of text.matchAll(declaration)) {
      let cursor = match.index + match[0].length
      const parameters = balanced(text, cursor, '(', ')')
      cursor += parameters.length
      const rest = text.slice(cursor, cursor + 2000)
      const header = rest.slice(0, Math.max(0, rest.search(/[{;]/)))
      const bases = header.trim().startsWith(':')
        ? header.trim().slice(1).split(/\bwhere\b/)[0].split(',').map(base => base.replace(/\(.*$/s, '').replace(/<.*$/s, '').trim().split('.').at(-1)).filter(Boolean)
        : []
      const bodyStart = cursor + rest.search(/[{;]/)
      const body = text[bodyStart] === '{' ? balanced(text, bodyStart, '{', '}') : ''
      const list = types.get(match[1]) ?? []
      list.push({ parameters, body, bases })
      types.set(match[1], list)
    }
  }
  return types
}

function declaresId(typeName, types, seen = new Set()) {
  if (seen.has(typeName))
    return false
  seen.add(typeName)
  for (const type of types.get(typeName) ?? []) {
    if (/[\s(,]Id\s*[,)=]/.test(type.parameters) || /\bId\s*\{\s*(?:get|init)/.test(type.body))
      return true
    if (type.bases.some(base => declaresId(base, types, seen)))
      return true
  }
  return false
}

/** 进得了查询串的标量类型（枚举另由 indexEnums 收集） */
const SCALAR_TYPES = new Set([
  'string',
  'bool',
  'byte',
  'sbyte',
  'short',
  'ushort',
  'int',
  'uint',
  'long',
  'ulong',
  'float',
  'double',
  'decimal',
  'char',
  'Guid',
  'DateTime',
  'DateTimeOffset',
  'DateOnly',
  'TimeOnly',
  'TimeSpan',
])
const COLLECTION_TYPES = /^(?:List|IList|IEnumerable|IReadOnlyList|IReadOnlyCollection|ICollection|HashSet|ISet|IReadOnlySet)<(.+)>$/

function indexEnums(files) {
  const enums = new Set()
  for (const file of files) {
    for (const match of readSource(file).matchAll(/\benum\s+(\w+)/g))
      enums.add(match[1])
  }
  return enums
}

/** 类型（去可空）是否标量；集合看元素；字典、类、记录都算嵌套对象 */
function isScalarType(typeText, enums) {
  const type = typeText.trim().replace(/\?$/, '').split('.').at(-1)
  if (SCALAR_TYPES.has(type) || enums.has(type))
    return true
  if (type.endsWith('[]'))
    return isScalarType(type.slice(0, -2), enums)
  const collection = COLLECTION_TYPES.exec(type)
  return collection ? isScalarType(collection[1], enums) : false
}

/** 属性声明里可以出现在类型之前的修饰符 */
const PROPERTY_MODIFIERS = new Set(['new', 'override', 'public', 'required', 'sealed', 'virtual'])

/** 「类型 名字」形式的声明里取类型：压缩空白后最后一个空格之前的部分；不是这种形式时返回 undefined */
function declaredType(declaration) {
  const text = declaration.replace(/\s+/g, ' ').trim()
  const space = text.lastIndexOf(' ')
  return space > 0 && /^\w+$/.test(text.slice(space + 1)) ? text.slice(0, space) : undefined
}

/** 记录位置参数的类型：先剥掉前置特性与默认值 */
function parameterType(parameter) {
  let text = parameter.trim()
  while (text.startsWith('[')) {
    const end = text.indexOf(']')
    if (end < 0)
      return undefined
    text = text.slice(end + 1).trim()
  }
  const assign = text.indexOf('=')
  return declaredType(assign < 0 ? text : text.slice(0, assign))
}

/** 一行公开实例属性（`public … 名字 { get` / `{ init`）的类型；静态属性不参与序列化，返回 undefined */
function propertyType(line) {
  const text = line.trim()
  const brace = text.indexOf('{')
  if (!text.startsWith('public ') || brace < 0 || !/^(?:get|init)\b/.test(text.slice(brace + 1).trimStart()))
    return undefined
  const words = text.slice(0, brace).trim().split(/\s+/)
  if (words.includes('static'))
    return undefined
  while (words.length > 0 && PROPERTY_MODIFIERS.has(words[0]))
    words.shift()
  return declaredType(words.join(' '))
}

/** 请求类型（含基类）的字段：记录位置参数与 { get / init } 属性 */
function fieldTypesOf(type) {
  const fields = []
  for (const parameter of splitTopLevel(type.parameters.slice(1, -1))) {
    const fieldType = parameterType(parameter)
    if (fieldType)
      fields.push(fieldType)
  }
  for (const line of type.body.split('\n')) {
    const fieldType = propertyType(line)
    if (fieldType)
      fields.push(fieldType)
  }
  return fields
}

/** 按顶层逗号切分（忽略尖括号与括号里的逗号） */
function splitTopLevel(text) {
  const parts = []
  let depth = 0
  let current = ''
  for (const char of text) {
    if (char === '<' || char === '(' || char === '[')
      depth++
    else if (char === '>' || char === ')' || char === ']')
      depth--
    if (char === ',' && depth === 0) {
      parts.push(current)
      current = ''
    }
    else {
      current += char
    }
  }
  if (current.trim())
    parts.push(current)
  return parts
}

/** 请求是否带嵌套对象字段：与 5.0 HttpRouteConvention.HasNestedFields 一致（GET / DELETE 不读请求体） */
function declaresNested(typeName, types, enums, seen = new Set()) {
  if (seen.has(typeName))
    return false
  seen.add(typeName)
  for (const type of types.get(typeName) ?? []) {
    if (fieldTypesOf(type).some(field => !isScalarType(field, enums)))
      return true
    if (type.bases.some(base => declaresNested(base, types, enums, seen)))
      return true
  }
  return false
}

function predicateMatch(name, prefix) {
  return name.startsWith(prefix) && (name.length === prefix.length || /[A-Z]/.test(name[prefix.length]))
}

/** 4.x：方法名前缀 → 谓词，没有匹配默认 POST */
function legacyVerb(methodName) {
  const prefix = PREFIXES_BY_LENGTH.find(p => predicateMatch(methodName, p))
  return prefix ? HTTP_PREFIXES[prefix] : 'POST'
}

/** 4.x：去 Async，再去动词前缀（剥完为空则保留原名） */
function legacyAction(methodName) {
  const name = methodName.endsWith('Async') ? methodName.slice(0, -5) : methodName
  for (const prefix of PREFIXES_BY_LENGTH) {
    if (predicateMatch(name, prefix) && name.length > prefix.length)
      return name.slice(prefix.length)
  }
  return name
}

/** 4.x：服务类型去 I 前缀与服务后缀 */
function legacyController(serviceType) {
  const name = /^I[A-Z]/.test(serviceType) ? serviceType.slice(1) : serviceType
  const suffix = SERVICE_SUFFIXES.find(s => name.endsWith(s))
  return suffix ? name.slice(0, -suffix.length) : name
}

/** 5.0 HttpRouteConvention（ADR-0015）：get / list / delete 的请求带嵌套对象时改用 POST 自定义动词 */
function deriveRoute(operationId, hasIdentifier, hasNestedFields) {
  const dot = operationId.indexOf('.')
  const resource = operationId.slice(0, dot)
  const verb = operationId.slice(dot + 1)
  const target = hasIdentifier ? `/${resource}/{id}` : `/${resource}`
  const route = (method, path) => ({ id: operationId, method, path: `${ROUTE_PREFIX}${path}` })
  switch (verb) {
    case 'create': return route('POST', `/${resource}`)
    case 'update': return route('PUT', target)
    case 'list': return hasNestedFields ? route('POST', `/${resource}:list`) : route('GET', `/${resource}`)
    case 'get': return hasNestedFields ? route('POST', `${target}:get`) : route('GET', target)
    case 'delete': return hasNestedFields ? route('POST', `${target}:delete`) : route('DELETE', target)
    default: return route('POST', `${target}:${verb}`)
  }
}

const moduleDirs = readdirSync(join(BACKEND_SRC, 'modules'), { withFileTypes: true }).filter(d => d.isDirectory()).map(d => join(BACKEND_SRC, 'modules', d.name))
const sourceFiles = listFiles(BACKEND_SRC, name => name.endsWith('.cs'))
const types = indexTypes(sourceFiles)
const enums = indexEnums(sourceFiles)
const operationFiles = moduleDirs.flatMap(dir => listFiles(join(dir, 'Operations'), name => name.endsWith('Operation.cs')))

const table = new Map()
const problems = []
for (const file of operationFiles) {
  const text = readSource(file)
  const id = /\[Operation\("([a-z0-9-]+\.[a-z0-9-]+)"/.exec(text)?.[1]
  const service = /class\s+\w+\s*\(\s*(\w+)\s+service\b/.exec(text)?.[1]
  const method = /\bservice\.(\w+)\s*\(/.exec(text)?.[1]
  const request = /IOperation<([\w.]+)\s*,/.exec(text)?.[1]?.split('.').at(-1)
  if (!id || !service || !method || !request) {
    problems.push(`${relative(BACKEND_SRC, file)}: 无法识别操作 / 服务 / 方法 / 请求类型`)
    continue
  }
  const key = `${legacyController(service)}/${legacyAction(method)}`
  const verb = legacyVerb(method)
  const routes = table.get(key) ?? {}
  if (routes[verb] && routes[verb].id !== id)
    problems.push(`${key} 的 ${verb} 同时对应 ${routes[verb].id} 与 ${id}`)
  // 生成的请求记录与操作在同一文件里，且不同资源下常有同名记录（GetRequest 等）：同文件里的声明优先
  const local = indexTypes([file]).get(request)
  const scoped = local ? new Map(types).set(request, local) : types
  routes[verb] = deriveRoute(id, declaresId(request, scoped), declaresNested(request, scoped, enums))
  table.set(key, routes)
}
for (const extra of EXTRA_ENDPOINTS) {
  const routes = table.get(extra.key) ?? {}
  routes[extra.verb] = extra.route
  table.set(extra.key, routes)
}
if (problems.length > 0) {
  console.error(`[operations] 发现 ${problems.length} 个问题：\n  ${problems.join('\n  ')}`)
  process.exit(1)
}

async function loadOpenApi(source) {
  if (/^https?:\/\//.test(source)) {
    const response = await fetch(source)
    if (!response.ok)
      throw new Error(`读取 ${source} 失败：HTTP ${response.status}`)
    return response.json()
  }
  return JSON.parse(readFileSync(source, 'utf8'))
}

if (verifySource) {
  const document = await loadOpenApi(verifySource)
  const served = new Map()
  for (const [path, operations] of Object.entries(document.paths ?? {})) {
    for (const [method, operation] of Object.entries(operations))
      served.set(operation.operationId, `${method.toUpperCase()} ${path}`)
  }
  const mismatches = []
  for (const routes of table.values()) {
    for (const route of Object.values(routes)) {
      if (EXTRA_ENDPOINTS.some(extra => extra.route.id === route.id))
        continue
      const expected = served.get(route.id)
      // 宿主未加载的模块（随 5.1 迁移的 AI / Workflow）不在 OpenAPI 里，不算不一致
      if (expected !== undefined && expected !== `${route.method} ${route.path}`)
        mismatches.push(`${route.id}: 推导 ${route.method} ${route.path}，宿主 ${expected}`)
    }
  }
  if (mismatches.length > 0) {
    console.error(`[operations] 推导与宿主 OpenAPI 不一致 ${mismatches.length} 处：\n  ${mismatches.join('\n  ')}`)
    process.exit(1)
  }
  console.log(`[operations] 与宿主 OpenAPI 核对一致（宿主操作 ${served.size} 个）`)
}

const quote = value => `'${value.replace(/\\/g, '\\\\').replace(/'/g, '\\\'')}'`
const lines = [
  '// 由 scripts/generate-operations.mjs 从后端 Operations 源码生成，不要手改；后端增删或改名操作后执行 pnpm api:operations。',
  '// 键是前端 facade 的「控制器/动作」（4.x 动态 API 写法），值按 4.x 谓词区分同名动作，路由是 5.0 操作的 HTTP 路由（已含前缀）。',
  '',
  'export type OperationMethod = \'DELETE\' | \'GET\' | \'POST\' | \'PUT\'',
  '',
  'export interface OperationRoute {',
  '  /** 5.0 操作 ID（<资源>.<动词>） */',
  '  id: string',
  '  /** HTTP 方法 */',
  '  method: OperationMethod',
  '  /** 路径模板（已含前缀）；实例级路由含 {id} */',
  '  path: string',
  '}',
  '',
  'export const OPERATIONS: Readonly<Record<string, Readonly<Partial<Record<OperationMethod | \'PATCH\', OperationRoute>>>>> = {',
]
for (const key of [...table.keys()].sort()) {
  const routes = table.get(key)
  const entries = Object.keys(routes).sort().map(verb => `${verb}: { id: ${quote(routes[verb].id)}, method: ${quote(routes[verb].method)}, path: ${quote(routes[verb].path)} }`)
  lines.push(`  ${quote(key)}: { ${entries.join(', ')} },`)
}
lines.push('}', '')
const content = lines.join('\n')

const current = existsSync(OUTPUT) ? readFileSync(OUTPUT, 'utf8') : ''
if (checkOnly) {
  if (current !== content) {
    console.error('[operations] src/api/operations.generated.ts 已过期：执行 pnpm api:operations 重新生成')
    process.exit(1)
  }
  console.log(`[operations] 路由表是最新的（${table.size} 个动作）`)
}
else {
  writeFileSync(OUTPUT, content)
  console.log(`[operations] 已生成 ${relative(FRONTEND, OUTPUT)}（${table.size} 个动作）`)
}
