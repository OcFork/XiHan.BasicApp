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
[Operation("profile.create-api-credential", Policy = "authenticated")]
public sealed class CreateApiCredentialOperation(ProfileAppService service) : IOperation<ProfileApiCredentialCreateDto, ProfileApiCredentialSecretDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileApiCredentialSecretDto>> HandleAsync(ProfileApiCredentialCreateDto request, InvocationContext context)
        => await service.CreateApiCredentialAsync(request, context.CancellationToken).ConfigureAwait(false);
}
