// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Application.QueryServices;
using XiHan.BasicApp.CodeGeneration.Domain.DomainServices;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Enums;
using XiHan.BasicApp.CodeGeneration.Domain.Generation;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Domain.Repositories;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Localization.Abstractions;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Serialization;

namespace XiHan.BasicApp.CodeGeneration.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
// 网络契约与 4.x 一致：枚举按名称、long 按字符串（雪花 ID 超过 2^53）、日期接受管理端控件的「yyyy-MM-dd HH:mm:ss」
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString,
    UseStringEnumConverter = true,
    Converters = new[] { typeof(Int64StringConverter), typeof(NullableInt64StringConverter), typeof(LenientDateTimeOffsetConverter), typeof(NullableLenientDateTimeOffsetConverter), typeof(LenientDateTimeConverter), typeof(NullableLenientDateTimeConverter) })]
[JsonSerializable(typeof(CodeGenConnectionTestResultDto))]
[JsonSerializable(typeof(CodeGenDataSourceCreateDto))]
[JsonSerializable(typeof(CodeGenDataSourceDetailDto))]
[JsonSerializable(typeof(CodeGenDataSourcePageQueryDto))]
[JsonSerializable(typeof(CodeGenDataSourceStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenDataSourceUpdateDto))]
[JsonSerializable(typeof(CodeGenDbTableQueryDto))]
[JsonSerializable(typeof(CodeGenGenerateRequestDto))]
[JsonSerializable(typeof(CodeGenHistoryDetailDto))]
[JsonSerializable(typeof(CodeGenHistoryPageQueryDto))]
[JsonSerializable(typeof(CodeGenImportTableDto))]
[JsonSerializable(typeof(CodeGenImportTablesDto))]
[JsonSerializable(typeof(CodeGenImportTablesResultDto))]
[JsonSerializable(typeof(CodeGenPreviewRequestDto))]
[JsonSerializable(typeof(CodeGenResultDto))]
[JsonSerializable(typeof(CodeGenSchemaSyncResultDto))]
[JsonSerializable(typeof(CodeGenTableColumnBatchSaveDto))]
[JsonSerializable(typeof(CodeGenTableColumnListItemDto))]
[JsonSerializable(typeof(CodeGenTableColumnUpdateDto))]
[JsonSerializable(typeof(CodeGenTableDetailDto))]
[JsonSerializable(typeof(CodeGenTablePageQueryDto))]
[JsonSerializable(typeof(CodeGenTableStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenTableUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateCreateDto))]
[JsonSerializable(typeof(CodeGenTemplateDetailDto))]
[JsonSerializable(typeof(CodeGenTemplatePageQueryDto))]
[JsonSerializable(typeof(CodeGenTemplateStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateValidateDto))]
[JsonSerializable(typeof(CodeGenTemplateValidateResultDto))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSource.DeleteRequest), TypeInfoPropertyName = "CodeGenDataSourceDeleteRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTable.DeleteRequest), TypeInfoPropertyName = "CodeGenTableDeleteRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplate.DeleteRequest), TypeInfoPropertyName = "CodeGenTemplateDeleteRequest")]
[JsonSerializable(typeof(DynamicRuntimePageQueryDto))]
[JsonSerializable(typeof(DynamicRuntimePageResultDto))]
[JsonSerializable(typeof(DynamicRuntimeSchemaDto))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.EmptyResponse))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenHistoryQuery.GetByTableRequest), TypeInfoPropertyName = "CodeGenHistoryQueryGetByTableRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableColumnQuery.GetByTableRequest), TypeInfoPropertyName = "CodeGenTableColumnQueryGetByTableRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSourceQuery.GetDetailRequest), TypeInfoPropertyName = "CodeGenDataSourceQueryGetDetailRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenHistoryQuery.GetDetailRequest), TypeInfoPropertyName = "CodeGenHistoryQueryGetDetailRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableQuery.GetDetailRequest), TypeInfoPropertyName = "CodeGenTableQueryGetDetailRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplateQuery.GetDetailRequest), TypeInfoPropertyName = "CodeGenTemplateQueryGetDetailRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSourceQuery.GetOptionsRequest), TypeInfoPropertyName = "CodeGenDataSourceQueryGetOptionsRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableQuery.GetOptionsRequest), TypeInfoPropertyName = "CodeGenTableQueryGetOptionsRequest")]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableQuery.GetParentMenuOptionsRequest))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.DynamicRuntime.GetSchemaRequest))]
[JsonSerializable(typeof(IReadOnlyList<CodeGenDataSourceListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<CodeGenHistoryListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<CodeGenParentMenuOptionDto>))]
[JsonSerializable(typeof(IReadOnlyList<CodeGenTableColumnListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<CodeGenTableListItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<string>))]
[JsonSerializable(typeof(PageResultDtoBase<CodeGenDataSourceListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<CodeGenHistoryListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<CodeGenTableListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<CodeGenTemplateListItemDto>))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGeneration.SyncSchemaRequest))]
[JsonSerializable(typeof(XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSource.TestConnectionRequest))]
internal sealed partial class CodeGenerationOperationsJsonContext : JsonSerializerContext;
