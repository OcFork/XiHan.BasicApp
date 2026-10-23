// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Application.Mappers;
using XiHan.BasicApp.AI.Application.QueryServices;
using XiHan.BasicApp.AI.Domain.Entities;
using XiHan.BasicApp.AI.Domain.Permissions;
using XiHan.BasicApp.AI.Domain.Repositories;
using XiHan.BasicApp.AI.Operations;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.AI.Operations.AiPromptQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("ai-prompt-query.list", Permission = AiPromptPermissionCodes.Read)]
public sealed class GetPageOperation(IAiPromptQueryService service) : IOperation<AiPromptPageQueryDto, PageResultDtoBase<AiPromptListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<AiPromptListItemDto>>> HandleAsync(AiPromptPageQueryDto request, InvocationContext context)
        => await service.GetPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
