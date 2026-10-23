// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Core.Upgrade.Abstractions;

/// <summary>
/// 升级租户提供者接口
/// </summary>
public interface IUpgradeTenantProvider
{
    /// <summary>
    /// 获取租户列表
    /// </summary>
    IReadOnlyList<BasicTenantInfo> GetTenants();
}
