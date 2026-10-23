// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;

namespace XiHan.BasicApp.CodeGeneration.Application;

/// <summary>
/// 代码生成应用服务基类
/// </summary>
/// <remarks>
/// 与 Saas 同构：类级 <see cref="AuthorizeAttribute"/> + <see cref="DynamicApiAttribute"/> 自动暴露为 HTTP API，
/// 但归属独立的 DynamicApi 分组，与系统 SaaS 服务区隔。
/// </remarks>
public abstract class CodeGenerationApplicationService
{
}
