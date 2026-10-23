// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Printing.Application.AppServices;
using XiHan.BasicApp.Printing.Application.Caching;
using XiHan.BasicApp.Printing.Application.Contracts;
using XiHan.BasicApp.Printing.Application.Dtos;
using XiHan.BasicApp.Printing.Application.Mappers;
using XiHan.BasicApp.Printing.Domain.DomainServices;
using XiHan.BasicApp.Printing.Domain.Entities;
using XiHan.BasicApp.Printing.Domain.Enums;
using XiHan.BasicApp.Printing.Domain.Permissions;
using XiHan.BasicApp.Printing.Operations;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Printing.Operations.PrintTemplate;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("print-template.update-status", Permission = PrintingPermissionCodes.Status)]
public sealed class UpdatePrintTemplateStatusOperation(IPrintTemplateAppService service) : IOperation<PrintTemplateStatusUpdateDto, PrintTemplateDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PrintTemplateDetailDto>> HandleAsync(PrintTemplateStatusUpdateDto request, InvocationContext context)
        => await service.UpdatePrintTemplateStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
