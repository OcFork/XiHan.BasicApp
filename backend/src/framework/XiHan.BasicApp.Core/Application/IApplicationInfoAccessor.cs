// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Application;

/// <summary>
/// 应用程序信息：名称与本实例标识（升级编排用它区分节点）
/// </summary>
public interface IApplicationInfoAccessor
{
    /// <summary>
    /// 应用名称
    /// </summary>
    string? ApplicationName { get; }

    /// <summary>
    /// 本实例标识（进程级唯一）
    /// </summary>
    string InstanceId { get; }
}
