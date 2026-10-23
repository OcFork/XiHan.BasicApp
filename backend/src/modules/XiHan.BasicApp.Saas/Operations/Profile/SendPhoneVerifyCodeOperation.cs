// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.send-phone-verify-code", Policy = "authenticated")]
public sealed class SendPhoneVerifyCodeOperation(ProfileAppService service) : IOperation<SendPhoneVerifyCodeRequest, ProfileVerificationCodeResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ProfileVerificationCodeResultDto>> HandleAsync(SendPhoneVerifyCodeRequest request, InvocationContext context)
        => await service.SendPhoneVerifyCodeAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record SendPhoneVerifyCodeRequest();
