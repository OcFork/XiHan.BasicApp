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
[Operation("enum-metadata.get-all-enums", Policy = "authenticated")]
public sealed class GetAllEnumsOperation(EnumMetadataAppService service) : IOperation<GetAllEnumsRequest, List<EnumMetadataDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<EnumMetadataDto>>> HandleAsync(GetAllEnumsRequest request, InvocationContext context)
        => await service.GetAllEnumsAsync().ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetAllEnumsRequest();
