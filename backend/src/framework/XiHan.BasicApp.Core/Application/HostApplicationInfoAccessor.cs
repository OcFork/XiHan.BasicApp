// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Hosting;

namespace XiHan.BasicApp.Core.Application;

/// <summary>
/// 从宿主环境取应用名，实例标识为机器名加进程标识加启动时生成的短随机段
/// </summary>
/// <param name="environment">宿主环境</param>
public sealed class HostApplicationInfoAccessor(IHostEnvironment environment) : IApplicationInfoAccessor
{
    /// <inheritdoc />
    public string? ApplicationName => environment.ApplicationName;

    /// <inheritdoc />
    public string InstanceId { get; } = $"{Environment.MachineName}:{Environment.ProcessId}:{Guid.NewGuid():N}"[..^24];
}
