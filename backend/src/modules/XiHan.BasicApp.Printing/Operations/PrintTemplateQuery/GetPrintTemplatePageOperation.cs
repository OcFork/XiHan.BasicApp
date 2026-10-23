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
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Printing.Operations.PrintTemplateQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("print-template-query.list-print-template", Permission = PrintingPermissionCodes.Read)]
public sealed class GetPrintTemplatePageOperation(IPrintTemplateQueryService service) : IOperation<PrintTemplatePageQueryDto, PageResultDtoBase<PrintTemplateListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<PrintTemplateListItemDto>>> HandleAsync(PrintTemplatePageQueryDto request, InvocationContext context)
        => await service.GetPrintTemplatePageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
