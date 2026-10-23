// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Hubs;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Repositories;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserSetting;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-setting.save", Policy = "authenticated")]
public sealed class SaveOperation(IUserSettingAppService service) : IOperation<UserSettingSaveDto, UserSettingDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<UserSettingDto>> HandleAsync(UserSettingSaveDto request, InvocationContext context)
        => await service.SaveAsync(request, context.CancellationToken).ConfigureAwait(false);
}
