// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Profile;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("profile.revoke-session", Policy = "authenticated")]
public sealed class RevokeSessionOperation(ProfileAppService service) : IOperation<ProfileSessionRevokeDto, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(ProfileSessionRevokeDto request, InvocationContext context)
    {
        await service.RevokeSessionAsync(request, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}
