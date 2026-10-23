// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Upgrade.Models;

namespace XiHan.BasicApp.Core.Upgrade.Abstractions;

/// <summary>
/// 升级协调器接口
/// </summary>
public interface IUpgradeCoordinator
{
    /// <summary>
    /// 启动升级（后台执行）
    /// </summary>
    /// <returns></returns>
    Task<UpgradeStartResult> StartAsync();
}
