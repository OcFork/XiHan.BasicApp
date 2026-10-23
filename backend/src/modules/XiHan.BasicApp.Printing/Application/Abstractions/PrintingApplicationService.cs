// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;

namespace XiHan.BasicApp.Printing.Application;

/// <summary>
/// 打印应用服务基类
/// </summary>
/// <remarks>
/// 与 Saas/AI/代码生成同构：类级 <see cref="AuthorizeAttribute"/> + <see cref="DynamicApiAttribute"/> 自动暴露为 HTTP API，
/// 归属独立的 DynamicApi 分组 BasicApp.Printing，与其它模块服务区隔。
/// </remarks>
public abstract class PrintingApplicationService
{
}
