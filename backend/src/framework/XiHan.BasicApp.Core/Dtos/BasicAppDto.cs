// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Paging.Dtos;

namespace XiHan.BasicApp.Core.Dtos;

/// <summary>
/// BasicApp DTO 基类
/// 命名规则：Dto（Data Transfer Object）
/// </summary>
/// <remarks>5.0 不再提供 DTO 基类，标识属性随实体协议叫 <c>Id</c>。</remarks>
public abstract class BasicAppDto
{
    /// <summary>
    /// 标识
    /// </summary>
    public virtual long Id { get; set; }
}

/// <summary>
/// BasicApp 创建 DTO 基类
/// 命名规则：CDto（Create DTO）
/// </summary>
public abstract class BasicAppCDto
{
}

/// <summary>
/// BasicApp 修改 DTO 基类
/// 命名规则：UDto（Update DTO）
/// </summary>
public abstract class BasicAppUDto
{
    /// <summary>
    /// 标识
    /// </summary>
    public virtual long Id { get; set; }
}

/// <summary>
/// BasicApp 分页请求 DTO 基类
/// 命名规则：PRDto（Page Request DTO）
/// </summary>
public class BasicAppPRDto : PageRequestDtoBase
{
}
