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
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSource;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSourceQuery;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenHistoryQuery;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTable;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableColumn;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableColumnQuery;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableQuery;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplate;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplateQuery;
using XiHan.BasicApp.CodeGeneration.Operations.CodeGeneration;
using XiHan.BasicApp.CodeGeneration.Operations.DynamicRuntime;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.MultiTenancy.Abstractions;

namespace XiHan.BasicApp.CodeGeneration.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(CodeGenConnectionTestResultDto))]
[JsonSerializable(typeof(CodeGenDataSourceCreateDto))]
[JsonSerializable(typeof(CodeGenDataSourceDetailDto))]
[JsonSerializable(typeof(CodeGenDataSourceDetailDto?))]
[JsonSerializable(typeof(CodeGenDataSourcePageQueryDto))]
[JsonSerializable(typeof(CodeGenDataSourceStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenDataSourceUpdateDto))]
[JsonSerializable(typeof(CodeGenDbTableQueryDto))]
[JsonSerializable(typeof(CodeGenGenerateRequestDto))]
[JsonSerializable(typeof(CodeGenHistoryDetailDto?))]
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
[JsonSerializable(typeof(CodeGenTableDetailDto?))]
[JsonSerializable(typeof(CodeGenTablePageQueryDto))]
[JsonSerializable(typeof(CodeGenTableStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenTableUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateCreateDto))]
[JsonSerializable(typeof(CodeGenTemplateDetailDto))]
[JsonSerializable(typeof(CodeGenTemplateDetailDto?))]
[JsonSerializable(typeof(CodeGenTemplatePageQueryDto))]
[JsonSerializable(typeof(CodeGenTemplateStatusUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateUpdateDto))]
[JsonSerializable(typeof(CodeGenTemplateValidateDto))]
[JsonSerializable(typeof(CodeGenTemplateValidateResultDto))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(DynamicRuntimePageQueryDto))]
[JsonSerializable(typeof(DynamicRuntimePageResultDto))]
[JsonSerializable(typeof(DynamicRuntimeSchemaDto))]
[JsonSerializable(typeof(EmptyResponse))]
[JsonSerializable(typeof(GetByTableRequest))]
[JsonSerializable(typeof(GetDetailRequest))]
[JsonSerializable(typeof(GetOptionsRequest))]
[JsonSerializable(typeof(GetParentMenuOptionsRequest))]
[JsonSerializable(typeof(GetSchemaRequest))]
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
[JsonSerializable(typeof(SyncSchemaRequest))]
[JsonSerializable(typeof(TestConnectionRequest))]
internal sealed partial class CodeGenerationOperationsJsonContext : JsonSerializerContext;
