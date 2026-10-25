// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Application.QueryServices;
using XiHan.BasicApp.Workflow.Domain.Entities;
using XiHan.BasicApp.Workflow.Domain.Permissions;
using XiHan.BasicApp.Workflow.Domain.Repositories;
using XiHan.BasicApp.Workflow.Infrastructure.Stores;
using XiHan.BasicApp.Workflow.Operations.WorkflowApproval;
using XiHan.BasicApp.Workflow.Operations.WorkflowApprovalQuery;
using XiHan.BasicApp.Workflow.Operations.WorkflowDefinition;
using XiHan.BasicApp.Workflow.Operations.WorkflowDefinitionQuery;
using XiHan.BasicApp.Workflow.Operations.WorkflowDirectoryQuery;
using XiHan.BasicApp.Workflow.Operations.WorkflowInstance;
using XiHan.BasicApp.Workflow.Operations.WorkflowInstanceQuery;
using XiHan.BasicApp.Workflow.Operations.WorkflowTodo;
using XiHan.BasicApp.Workflow.Operations.WorkflowTodoQuery;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Serialization;

namespace XiHan.BasicApp.Workflow.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
// 网络契约与 4.x 一致：枚举按名称、long 按字符串（雪花 ID 超过 2^53）、日期接受管理端控件的「yyyy-MM-dd HH:mm:ss」
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString,
    UseStringEnumConverter = true,
    Converters = new[] { typeof(Int64StringConverter), typeof(NullableInt64StringConverter), typeof(LenientDateTimeOffsetConverter), typeof(NullableLenientDateTimeOffsetConverter), typeof(LenientDateTimeConverter), typeof(NullableLenientDateTimeConverter) })]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(EmptyResponse))]
[JsonSerializable(typeof(GetBusinessTypesRequest))]
[JsonSerializable(typeof(GetDepartmentTreeRequest))]
[JsonSerializable(typeof(GetDetailRequest))]
[JsonSerializable(typeof(GetPositionOptionsRequest))]
[JsonSerializable(typeof(GetRoleOptionsRequest))]
[JsonSerializable(typeof(List<WorkflowBusinessTypeDto>))]
[JsonSerializable(typeof(List<WorkflowDepartmentOptionDto>))]
[JsonSerializable(typeof(List<WorkflowOrganizationOptionDto>))]
[JsonSerializable(typeof(List<WorkflowUserOptionDto>))]
[JsonSerializable(typeof(PageResultDtoBase<WorkflowApprovalInstanceItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<WorkflowApprovalRecordItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<WorkflowDefinitionListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<WorkflowInstanceListItemDto>))]
[JsonSerializable(typeof(PageResultDtoBase<WorkflowTodoListItemDto>))]
[JsonSerializable(typeof(WorkflowApprovalDetailDto))]
[JsonSerializable(typeof(WorkflowApprovalPageQueryDto))]
[JsonSerializable(typeof(WorkflowApprovalWithdrawDto))]
[JsonSerializable(typeof(WorkflowDefinitionCreateDto))]
[JsonSerializable(typeof(WorkflowDefinitionDetailDto))]
[JsonSerializable(typeof(WorkflowDefinitionIdDto))]
[JsonSerializable(typeof(WorkflowDefinitionNewVersionDto))]
[JsonSerializable(typeof(WorkflowDefinitionPageQueryDto))]
[JsonSerializable(typeof(WorkflowDefinitionUpdateDraftDto))]
[JsonSerializable(typeof(WorkflowInstanceDetailDto))]
[JsonSerializable(typeof(WorkflowInstanceIdDto))]
[JsonSerializable(typeof(WorkflowInstanceListItemDto))]
[JsonSerializable(typeof(WorkflowInstanceOperationDto))]
[JsonSerializable(typeof(WorkflowInstancePageQueryDto))]
[JsonSerializable(typeof(WorkflowInstanceStartDto))]
[JsonSerializable(typeof(WorkflowSignalPublishDto))]
[JsonSerializable(typeof(WorkflowSignalPublishResultDto))]
[JsonSerializable(typeof(WorkflowTodoAddAssigneesDto))]
[JsonSerializable(typeof(WorkflowTodoCompleteDto))]
[JsonSerializable(typeof(WorkflowTodoCompleteResultDto))]
[JsonSerializable(typeof(WorkflowTodoPageQueryDto))]
[JsonSerializable(typeof(WorkflowTodoTransferDto))]
[JsonSerializable(typeof(WorkflowUserOptionQueryDto))]
internal sealed partial class WorkflowOperationsJsonContext : JsonSerializerContext;
