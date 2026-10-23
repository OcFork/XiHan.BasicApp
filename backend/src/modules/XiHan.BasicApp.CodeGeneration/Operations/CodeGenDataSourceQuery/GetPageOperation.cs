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
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSourceQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-data-source-query.list", Permission = CodeGenPermissionCodes.Read)]
public sealed class GetPageOperation(ICodeGenDataSourceQueryService service) : IOperation<CodeGenDataSourcePageQueryDto, PageResultDtoBase<CodeGenDataSourceListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<CodeGenDataSourceListItemDto>>> HandleAsync(CodeGenDataSourcePageQueryDto request, InvocationContext context)
        => await service.GetPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
