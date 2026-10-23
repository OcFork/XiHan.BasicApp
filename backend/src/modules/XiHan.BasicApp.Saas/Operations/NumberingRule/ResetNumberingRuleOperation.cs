// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.NumberingRule;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("numbering-rule.reset", Permission = SaasPermissionCodes.Numbering.Reset)]
public sealed class ResetNumberingRuleOperation(INumberingRuleAppService service) : IOperation<NumberingRuleResetDto, NumberingRuleDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<NumberingRuleDetailDto>> HandleAsync(NumberingRuleResetDto request, InvocationContext context)
        => await service.ResetNumberingRuleAsync(request, context.CancellationToken).ConfigureAwait(false);
}
