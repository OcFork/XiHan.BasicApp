// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.update-api-credential-status", Policy = "authenticated")]
public sealed class UpdateApiCredentialStatusOperation(ProfileAppService service) : IOperation<ProfileApiCredentialStatusDto, ProfileApiCredentialDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileApiCredentialDto>> HandleAsync(ProfileApiCredentialStatusDto request, InvocationContext context)
        => await service.UpdateApiCredentialStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
