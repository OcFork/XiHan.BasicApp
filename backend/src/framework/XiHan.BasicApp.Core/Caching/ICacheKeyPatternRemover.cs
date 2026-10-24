// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 按键模式删除：5.0 缓存存储没有模式删除，由知道存储布局的实现（如 Saas 的 Redis 实现）提供
/// </summary>
// xihan-migrate(P5-09): 框架缺口——ICacheStore 没有按前缀/模式删除；补上后这里改为直接调用框架能力。
public interface ICacheKeyPatternRemover
{
    /// <summary>
    /// 删除匹配模式的键
    /// </summary>
    /// <param name="pattern">规范化后的键模式（不含存储自身的前缀），<c>*</c> 通配</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>删除的键数</returns>
    ValueTask<long> RemoveByPatternAsync(string pattern, CancellationToken cancellationToken);
}
