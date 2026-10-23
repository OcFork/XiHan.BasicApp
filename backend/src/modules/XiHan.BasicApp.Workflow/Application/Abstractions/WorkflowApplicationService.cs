// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;

namespace XiHan.BasicApp.Workflow.Application;

/// <summary>
/// 工作流应用服务基类
/// </summary>
/// <remarks>
/// 与 Saas 同构：类级 <see cref="AuthorizeAttribute"/> + <see cref="DynamicApiAttribute"/> 自动暴露为 HTTP API，
/// 归属独立的 DynamicApi 分组。工作流写操作不挂 UnitOfWork：引擎存储按操作独立作用域持久化，
/// 与请求事务无关（引擎不集成工作单元，见框架工作流库说明）。
/// </remarks>
public abstract class WorkflowApplicationService
{
}
