// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Permissions;

namespace XiHan.BasicApp.Saas.Application.AppServices.Monitoring;

/// <summary>
/// 缓存管理服务（查询 / 改字符串值 / 删除）
/// 读取走 saas:cache:read，写入与删除走 saas:cache:clear。
/// </summary>
public class CacheAppService
{
    private readonly ICacheManagementService _cacheManagementService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public CacheAppService(ICacheManagementService cacheManagementService)
    {
        _cacheManagementService = cacheManagementService;
    }

    /// <summary>
    /// 判断缓存键是否存在
    /// </summary>
    public bool Exists(string key)
    {
        return _cacheManagementService.Exists(key);
    }

    /// <summary>
    /// 按模式获取缓存键列表
    /// </summary>
    public IReadOnlyCollection<string> GetKeys(string pattern = "*")
    {
        return _cacheManagementService.GetKeys(pattern);
    }

    /// <summary>
    /// 获取缓存字符串值
    /// </summary>
    public string? GetString(string key)
    {
        return _cacheManagementService.GetString(key);
    }

    /// <summary>
    /// 更新缓存字符串值（鉴权关键命名空间禁止改写）
    /// </summary>
    public void UpdateString(CacheStringUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        _cacheManagementService.SetString(input.Key, input.Value);
    }

    /// <summary>
    /// 删除指定缓存键
    /// </summary>
    public void Remove(string key)
    {
        _cacheManagementService.Remove(key);
    }

    /// <summary>
    /// 按模式批量删除缓存键
    /// </summary>
    /// <remarks>缓存管理页的「按模式删除」调用此处；动态 API 剥离 Remove 前缀，实际路由为 /Cache/ByPattern。</remarks>
    public long RemoveByPattern(string pattern = "*")
    {
        return _cacheManagementService.RemoveByPattern(pattern);
    }

}
