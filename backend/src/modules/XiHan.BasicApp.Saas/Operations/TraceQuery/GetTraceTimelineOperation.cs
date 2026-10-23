// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.TraceQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("trace-query.get-trace-timeline", Permission = SaasPermissionCodes.LogTrace.Read)]
public sealed class GetTraceTimelineOperation(ITraceQueryService service) : IOperation<TraceTimelineQueryDto, TraceTimelineResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<TraceTimelineResultDto>> HandleAsync(TraceTimelineQueryDto request, InvocationContext context)
        => await service.GetTraceTimelineAsync(request, context.CancellationToken).ConfigureAwait(false);
}
