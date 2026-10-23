// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Text.Json.Serialization;
using XiHan.BasicApp.Chat.Application.AppServices;
using XiHan.BasicApp.Chat.Application.Contracts;
using XiHan.BasicApp.Chat.Application.Dtos;
using XiHan.BasicApp.Chat.Application.Mappers;
using XiHan.BasicApp.Chat.Application.QueryServices;
using XiHan.BasicApp.Chat.Application.Services;
using XiHan.BasicApp.Chat.Domain.DomainServices;
using XiHan.BasicApp.Chat.Domain.Entities;
using XiHan.BasicApp.Chat.Domain.Permissions;
using XiHan.BasicApp.Chat.Domain.Repositories;
using XiHan.BasicApp.Chat.Operations.Chat;
using XiHan.BasicApp.Chat.Operations.ChatAuditQuery;
using XiHan.BasicApp.Chat.Operations.ChatQuery;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.MultiTenancy.Abstractions;

namespace XiHan.BasicApp.Chat.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(ChatAuditPageQueryDto))]
[JsonSerializable(typeof(ChatConversationDto))]
[JsonSerializable(typeof(ChatConversationInfoUpdateDto))]
[JsonSerializable(typeof(ChatConversationLeaveDto))]
[JsonSerializable(typeof(ChatConversationToggleDto))]
[JsonSerializable(typeof(ChatDepartmentConversationOpenDto))]
[JsonSerializable(typeof(ChatGroupCreateDto))]
[JsonSerializable(typeof(ChatMarkReadDto))]
[JsonSerializable(typeof(ChatMemberAddDto))]
[JsonSerializable(typeof(ChatMemberRemoveDto))]
[JsonSerializable(typeof(ChatMemberRoleDto))]
[JsonSerializable(typeof(ChatMemberSilenceDto))]
[JsonSerializable(typeof(ChatMessageEditDto))]
[JsonSerializable(typeof(ChatMessageHistoryQueryDto))]
[JsonSerializable(typeof(ChatMessageHistoryResultDto))]
[JsonSerializable(typeof(ChatMessageItemDto))]
[JsonSerializable(typeof(ChatMessagePinDto))]
[JsonSerializable(typeof(ChatMessageSearchQueryDto))]
[JsonSerializable(typeof(ChatMessageSendDto))]
[JsonSerializable(typeof(ChatOwnerTransferDto))]
[JsonSerializable(typeof(ChatReactionToggleDto))]
[JsonSerializable(typeof(ChatReactionToggleResultDto))]
[JsonSerializable(typeof(ChatSingleConversationOpenDto))]
[JsonSerializable(typeof(ChatToggleStateDto))]
[JsonSerializable(typeof(EmptyResponse))]
[JsonSerializable(typeof(GetDepartmentTreeRequest))]
[JsonSerializable(typeof(GetMembersRequest))]
[JsonSerializable(typeof(GetMyConversationsRequest))]
[JsonSerializable(typeof(GetPinnedMessagesRequest))]
[JsonSerializable(typeof(GetReadPositionsRequest))]
[JsonSerializable(typeof(IReadOnlyList<DepartmentTreeNodeDto>))]
[JsonSerializable(typeof(IReadOnlyList<UserSelectItemDto>))]
[JsonSerializable(typeof(List<ChatConversationListItemDto>))]
[JsonSerializable(typeof(List<ChatMemberItemDto>))]
[JsonSerializable(typeof(List<ChatMessageItemDto>))]
[JsonSerializable(typeof(List<ChatReadPositionDto>))]
[JsonSerializable(typeof(PageResultDtoBase<ChatAuditListItemDto>))]
[JsonSerializable(typeof(RecallMessageRequest))]
[JsonSerializable(typeof(UserSelectQueryDto))]
internal sealed partial class ChatOperationsJsonContext : JsonSerializerContext;
