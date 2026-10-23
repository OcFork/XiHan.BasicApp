// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using XiHan.BasicApp.AI.Application.AppServices;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Application.Mappers;
using XiHan.BasicApp.AI.Domain.Permissions;
using XiHan.BasicApp.AI.Infrastructure.Configuration;
using XiHan.BasicApp.AI.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.AI.Operations.KnowledgeQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("knowledge-query.list", Permission = KnowledgePermissionCodes.Execute)]
public sealed class QueryOperation(IKnowledgeQueryAppService service) : IOperation<KnowledgeQueryDto, KnowledgeQueryResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<KnowledgeQueryResultDto>> HandleAsync(KnowledgeQueryDto request, InvocationContext context)
        => await service.QueryAsync(request, context.CancellationToken).ConfigureAwait(false);
}
