// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.send2-fa-setup-code", Policy = "authenticated")]
public sealed class Send2FASetupCodeOperation(ProfileAppService service) : IOperation<ProfileTwoFactorMethodDto, ProfileVerificationCodeResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileVerificationCodeResultDto>> HandleAsync(ProfileTwoFactorMethodDto request, InvocationContext context)
        => await service.Send2FASetupCodeAsync(request, context.CancellationToken).ConfigureAwait(false);
}
