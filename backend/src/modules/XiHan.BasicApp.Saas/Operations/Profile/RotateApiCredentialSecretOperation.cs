// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Operations;
using XiHan.BasicApp.Core.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.rotate-api-credential-secret", Policy = "authenticated")]
public sealed class RotateApiCredentialSecretOperation(ProfileAppService service) : IOperation<ProfileApiCredentialIdDto, ProfileApiCredentialSecretDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileApiCredentialSecretDto>> HandleAsync(ProfileApiCredentialIdDto request, InvocationContext context)
        => await service.RotateApiCredentialSecretAsync(request, context.CancellationToken).ConfigureAwait(false);
}
