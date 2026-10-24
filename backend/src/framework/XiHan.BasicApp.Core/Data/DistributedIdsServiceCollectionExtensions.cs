// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using XiHan.BasicApp.Core.Data.Guids;
using XiHan.BasicApp.Core.Data.NanoIds;
using XiHan.BasicApp.Core.Data.SnowflakeIds;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 分布式 ID 注册（承接 4.x DistributedIds 模块的 ConfigureServices）
/// </summary>
public static class DistributedIdsServiceCollectionExtensions
{
    /// <summary>
    /// 注册雪花 ID（<see cref="IDistributedIdGenerator{T}"/> of <see cref="long"/>）与顺序 GUID 生成器，参数来自配置节 <c>XiHan:DistributedIds</c>
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddXiHanDistributedIds(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // 基线默认值（高负载预设）→ 再由配置节覆盖；Configure 先、Bind 后，后者才能覆盖前者。
        // 多节点集群必须在配置里给每个节点不同的 WorkerId，否则必然产生重复 ID
        services.AddOptions<SnowflakeIdOptions>()
            .Configure(options =>
            {
                options.WorkerIdBitLength = 6;
                options.SeqBitLength = 12;
                options.WorkerId = 1;
            })
            .Bind(configuration.GetSection(SnowflakeIdOptions.SectionName));
        services.Configure<SequentialGuidOptions>(configuration.GetSection(SequentialGuidOptions.SectionName));
        services.Configure<NanoIdOptions>(configuration.GetSection(NanoIdOptions.SectionName));

        services.TryAddSingleton(static sp =>
            IdGeneratorFactory.CreateSequentialGuidGenerator(sp.GetRequiredService<IOptions<SequentialGuidOptions>>().Value));
        services.TryAddSingleton(static sp =>
            IdGeneratorFactory.CreateSnowflakeIdGenerator(sp.GetRequiredService<IOptions<SnowflakeIdOptions>>().Value));
        return services;
    }
}
