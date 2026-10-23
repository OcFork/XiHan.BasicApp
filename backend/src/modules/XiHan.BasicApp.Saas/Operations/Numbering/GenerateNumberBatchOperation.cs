// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Numbering;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("numbering.generate-number-batch", Permission = SaasPermissionCodes.Numbering.Generate)]
public sealed class GenerateNumberBatchOperation(INumberingAppService service) : IOperation<NumberBatchGenerateDto, NumberGenerationResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<NumberGenerationResultDto>> HandleAsync(NumberBatchGenerateDto request, InvocationContext context)
        => await service.GenerateNumberBatchAsync(request, context.CancellationToken).ConfigureAwait(false);
}
