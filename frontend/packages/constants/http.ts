// ==================== HTTP 状态码 ====================

export const HTTP_STATUS = {
  OK: 200,
  CREATED: 201,
  NO_CONTENT: 204,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  /** 会话已锁屏：身份仍有效，客户端应展示锁屏而**不是**跳登录页（区别于 401） */
  LOCKED: 423,
  INTERNAL_SERVER_ERROR: 500,
} as const
