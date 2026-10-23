// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices.Metadata;
using XiHan.BasicApp.Saas.Application.Dtos.Metadata;
using XiHan.BasicApp.Saas.Domain.Numbering;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.TimeZoneMetadata;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("time-zone-metadata.get-time-zone-options", Policy = "authenticated")]
public sealed class GetTimeZoneOptionsOperation(TimeZoneMetadataAppService service) : IOperation<GetTimeZoneOptionsRequest, IReadOnlyList<TimeZoneOptionDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<TimeZoneOptionDto>>> HandleAsync(GetTimeZoneOptionsRequest request, InvocationContext context)
        => await service.GetTimeZoneOptionsAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetTimeZoneOptionsRequest();
