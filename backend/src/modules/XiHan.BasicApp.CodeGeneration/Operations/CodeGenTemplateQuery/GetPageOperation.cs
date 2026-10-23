// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Application.QueryServices;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Domain.Repositories;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplateQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-template-query.list", Permission = CodeGenPermissionCodes.Read)]
public sealed class GetPageOperation(ICodeGenTemplateQueryService service) : IOperation<CodeGenTemplatePageQueryDto, PageResultDtoBase<CodeGenTemplateListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<CodeGenTemplateListItemDto>>> HandleAsync(CodeGenTemplatePageQueryDto request, InvocationContext context)
        => await service.GetPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
