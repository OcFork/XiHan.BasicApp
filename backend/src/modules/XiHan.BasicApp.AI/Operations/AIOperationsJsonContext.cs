// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json.Serialization;
using XiHan.BasicApp.AI.Application.AppServices;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Application.Mappers;
using XiHan.BasicApp.AI.Application.QueryServices;
using XiHan.BasicApp.AI.Domain.DomainServices;
using XiHan.BasicApp.AI.Domain.Entities;
using XiHan.BasicApp.AI.Domain.Permissions;
using XiHan.BasicApp.AI.Domain.Repositories;
using XiHan.BasicApp.AI.Infrastructure.Configuration;
using XiHan.BasicApp.AI.Operations.AiAssistant;
using XiHan.BasicApp.AI.Operations.AiAssistantQuery;
using XiHan.BasicApp.AI.Operations.AiPrompt;
using XiHan.BasicApp.AI.Operations.AiPromptQuery;
using XiHan.BasicApp.AI.Operations.AiProvider;
using XiHan.BasicApp.AI.Operations.AiProviderQuery;
using XiHan.BasicApp.AI.Operations.ChatAssistant;
using XiHan.BasicApp.AI.Operations.KnowledgeDocument;
using XiHan.BasicApp.AI.Operations.KnowledgeDocumentQuery;
using XiHan.BasicApp.AI.Operations.KnowledgeQuery;
using XiHan.BasicApp.Chat.Application.Mappers;
using XiHan.BasicApp.Chat.Application.Services;
using XiHan.BasicApp.Chat.Domain.DomainServices;
using XiHan.BasicApp.Chat.Domain.Entities;
using XiHan.BasicApp.Chat.Domain.Permissions;
using XiHan.BasicApp.Chat.Domain.Repositories;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.MultiTenancy.Abstractions;

namespace XiHan.BasicApp.AI.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(AiAssistantActionDto))]
[JsonSerializable(typeof(AiAssistantCreateDto))]
[JsonSerializable(typeof(AiAssistantDetailDto))]
[JsonSerializable(typeof(AiAssistantDetailDto?))]
[JsonSerializable(typeof(AiAssistantPageQueryDto))]
[JsonSerializable(typeof(AiAssistantStatusUpdateDto))]
[JsonSerializable(typeof(AiAssistantUpdateDto))]
[JsonSerializable(typeof(AiPromptCreateDto))]
[JsonSerializable(typeof(AiPromptDetailDto))]
[JsonSerializable(typeof(AiPromptDetailDto?))]
[JsonSerializable(typeof(AiPromptPageQueryDto))]
[JsonSerializable(typeof(AiPromptStatusUpdateDto))]
[JsonSerializable(typeof(AiPromptUpdateDto))]
[JsonSerializable(typeof(AiProviderActionDto))]
[JsonSerializable(typeof(AiProviderCreateDto))]
[JsonSerializable(typeof(AiProviderDetailDto))]
[JsonSerializable(typeof(AiProviderDetailDto?))]
[JsonSerializable(typeof(AiProviderPageQueryDto))]
[JsonSerializable(typeof(AiProviderStatusUpdateDto))]
[JsonSerializable(typeof(AiProviderTestConnectionResultDto))]
[JsonSerializable(typeof(AiProviderUpdateDto))]
[JsonSerializable(typeof(ChatAssistantConversationDto))]
[JsonSerializable(typeof(ChatAssistantOpenDto))]
[JsonSerializable(typeof(ChatAssistantReplyDto))]
[JsonSerializable(typeof(ChatAssistantReplyResultDto))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(EmptyResponse))]
[JsonSerializable(typeof(GetAvailableRequest))]
[JsonSerializable(typeof(GetDetailRequest))]
[JsonSerializable(typeof(IReadOnlyList<AiAssistantOptionDto>))]
[JsonSerializable(typeof(KnowledgeActionDto))]
[JsonSerializable(typeof(KnowledgeDetailDto))]
[JsonSerializable(typeof(KnowledgeDetailDto?))]
[JsonSerializable(typeof(KnowledgeIngestDto))]
[JsonSerializable(typeof(KnowledgePageQueryDto))]
[JsonSerializable(typeof(KnowledgeQueryDto))]
[JsonSerializable(typeof(KnowledgeQueryResultDto))]
[JsonSerializable(typeof(PageResultDtoBase<AiAssistantListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<AiPromptListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<AiProviderListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<KnowledgeListItemDto>))]
internal sealed partial class AIOperationsJsonContext : JsonSerializerContext;
