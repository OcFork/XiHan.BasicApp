// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Printing.Application.Contracts;
using XiHan.BasicApp.Printing.Application.Dtos;
using XiHan.BasicApp.Printing.Application.Mappers;
using XiHan.BasicApp.Printing.Application.QueryServices;
using XiHan.BasicApp.Printing.Domain.Entities;
using XiHan.BasicApp.Printing.Domain.Enums;
using XiHan.BasicApp.Printing.Domain.Permissions;
using XiHan.BasicApp.Printing.Domain.Repositories;
using XiHan.BasicApp.Printing.Operations;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Printing.Operations.PrintTemplateQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("print-template-query.get-resolved-print-template-by-code", Permission = PrintingPermissionCodes.Use)]
public sealed class GetResolvedPrintTemplateByCodeOperation(IPrintTemplateQueryService service) : IOperation<GetResolvedPrintTemplateByCodeRequest, ResolvedPrintTemplateDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ResolvedPrintTemplateDto>> HandleAsync(GetResolvedPrintTemplateByCodeRequest request, InvocationContext context)
        => await service.GetResolvedPrintTemplateByCodeAsync(request.TemplateCode, request.Scope, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetResolvedPrintTemplateByCodeRequest(string TemplateCode, PrintTemplateScope Scope = PrintTemplateScope.Auto);
