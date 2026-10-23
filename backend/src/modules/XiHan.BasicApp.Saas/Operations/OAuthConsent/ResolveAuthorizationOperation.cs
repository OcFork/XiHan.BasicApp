// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.OAuthConsent;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("o-auth-consent.resolve-authorization", Policy = "authenticated")]
public sealed class ResolveAuthorizationOperation(IOAuthConsentAppService service) : IOperation<OAuthAuthorizeRequestDto, OAuthConsentPreviewDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<OAuthConsentPreviewDto>> HandleAsync(OAuthAuthorizeRequestDto request, InvocationContext context)
        => await service.ResolveAuthorizationAsync(request, context.CancellationToken).ConfigureAwait(false);
}
