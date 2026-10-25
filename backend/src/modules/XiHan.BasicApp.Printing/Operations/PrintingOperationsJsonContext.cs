// xihan-migrate(V4-06): 在特性的 Configure 里调用 context.AddGeneratedOperations()；操作委托给原服务类，V4Services 文件已把它们按作用域注册。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Printing.Application.AppServices;
using XiHan.BasicApp.Printing.Application.Caching;
using XiHan.BasicApp.Printing.Application.Contracts;
using XiHan.BasicApp.Printing.Application.Dtos;
using XiHan.BasicApp.Printing.Application.Mappers;
using XiHan.BasicApp.Printing.Application.QueryServices;
using XiHan.BasicApp.Printing.Domain.DataSources;
using XiHan.BasicApp.Printing.Domain.DomainServices;
using XiHan.BasicApp.Printing.Domain.Entities;
using XiHan.BasicApp.Printing.Domain.Enums;
using XiHan.BasicApp.Printing.Domain.Permissions;
using XiHan.BasicApp.Printing.Domain.Repositories;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Serialization;

namespace XiHan.BasicApp.Printing.Operations;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
// 网络契约与 4.x 一致：枚举按名称、long 按字符串（雪花 ID 超过 2^53）、日期接受管理端控件的「yyyy-MM-dd HH:mm:ss」
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString,
    UseStringEnumConverter = true,
    Converters = new[] { typeof(Int64StringConverter), typeof(NullableInt64StringConverter), typeof(LenientDateTimeOffsetConverter), typeof(NullableLenientDateTimeOffsetConverter), typeof(LenientDateTimeConverter), typeof(NullableLenientDateTimeConverter) })]
[JsonSerializable(typeof(XiHan.BasicApp.Printing.Operations.EmptyResponse))]
[JsonSerializable(typeof(XiHan.BasicApp.Printing.Operations.PrintDataSourceQuery.GetListRequest))]
[JsonSerializable(typeof(XiHan.BasicApp.Printing.Operations.PrintTemplateQuery.GetPrintTemplateDetailRequest))]
[JsonSerializable(typeof(XiHan.BasicApp.Printing.Operations.PrintTemplateQuery.GetResolvedPrintTemplateByCodeRequest))]
[JsonSerializable(typeof(List<PrintDataSourceDto>))]
[JsonSerializable(typeof(PageResultDtoBase<PrintTemplateListItemDto>))]
[JsonSerializable(typeof(PrintTemplateCreateDto))]
[JsonSerializable(typeof(PrintTemplateDeleteDto))]
[JsonSerializable(typeof(PrintTemplateDetailDto))]
[JsonSerializable(typeof(PrintTemplatePageQueryDto))]
[JsonSerializable(typeof(PrintTemplateStatusUpdateDto))]
[JsonSerializable(typeof(PrintTemplateUpdateDto))]
[JsonSerializable(typeof(ResolvedPrintTemplateDto))]
internal sealed partial class PrintingOperationsJsonContext : JsonSerializerContext;
