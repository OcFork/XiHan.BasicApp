// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices.Metadata;
using XiHan.BasicApp.Saas.Application.Dtos.Metadata;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.EnumMetadata;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("enum-metadata.get-enum", Policy = "authenticated")]
public sealed class GetEnumOperation(EnumMetadataAppService service) : IOperation<GetEnumRequest, EnumMetadataDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<EnumMetadataDto>> HandleAsync(GetEnumRequest request, InvocationContext context)
        => await service.GetEnumAsync(request.EnumTypeName).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetEnumRequest(string EnumTypeName);
