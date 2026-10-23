// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Upgrade.Models;

namespace XiHan.BasicApp.Core.Upgrade.Abstractions;

/// <summary>
/// 升级脚本提供者接口
/// </summary>
public interface IUpgradeScriptProvider
{
    /// <summary>
    /// 获取升级脚本列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<UpgradeScript>> GetScriptsAsync(CancellationToken cancellationToken = default);
}
