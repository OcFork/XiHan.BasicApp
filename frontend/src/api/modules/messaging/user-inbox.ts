import type { UserInboxItemDto, UserInboxUpdateDto } from './user-inbox.types'
import { createDynamicApiClient } from '../../base'

const userInboxDynamicApi = createDynamicApiClient('UserInbox')

export const userInboxApi = {
  banner() {
    return userInboxDynamicApi.get<UserInboxItemDto[]>('Banner')
  },
  confirm(id: string) {
    return userInboxDynamicApi.post<void, UserInboxUpdateDto>('Confirm', toUpdateDto(id))
  },
  list(unreadOnly = false) {
    return userInboxDynamicApi.get<UserInboxItemDto[]>('List', { unreadOnly })
  },
  mandatoryUnread() {
    return userInboxDynamicApi.get<UserInboxItemDto[]>('MandatoryUnread')
  },
  markAllRead() {
    return userInboxDynamicApi.post<void>('MarkAllRead')
  },
  markPopupShown(id: string) {
    return userInboxDynamicApi.post<void, UserInboxUpdateDto>('MarkPopupShown', toUpdateDto(id))
  },
  markRead(id: string) {
    return userInboxDynamicApi.post<void, UserInboxUpdateDto>('MarkRead', toUpdateDto(id))
  },
  popup() {
    return userInboxDynamicApi.get<UserInboxItemDto[]>('Popup')
  },
}

function toUpdateDto(id: string): UserInboxUpdateDto {
  return { id }
}
