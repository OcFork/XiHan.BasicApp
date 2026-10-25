// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using XiHan.BasicApp.Core.ObjectStorage.Constants;
using XiHan.BasicApp.Core.ObjectStorage.Options;
using XiHan.BasicApp.Core.ObjectStorage.Providers;
using XiHan.BasicApp.Core.ObjectStorage.Services;
using XiHan.Framework.ObjectStorage.S3;

namespace XiHan.BasicApp.Core.ObjectStorage.Extensions.DependencyInjection;

/// <summary>
/// 对象存储服务注册扩展
/// </summary>
public static class XiHanObjectStorageServiceCollectionExtensions
{
    /// <summary>
    /// 添加对象存储全部服务（从配置文件绑定选项并自动注册已启用的 Provider）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">应用配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddXiHanObjectStorage(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddXiHanObjectStorage();
        services.Configure<XiHanObjectStorageOptions>(configuration.GetSection(XiHanObjectStorageOptions.SectionName));
        services.Configure<LocalStorageOptions>(configuration.GetSection(LocalStorageOptions.SectionName));
        services.Configure<S3StorageOptions>(configuration.GetSection(S3StorageOptions.SectionName));

        RegisterConfiguredProviders(services, configuration);
        return services;
    }

    /// <summary>
    /// 添加对象存储核心服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">对象存储配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddXiHanObjectStorage(this IServiceCollection services, Action<XiHanObjectStorageOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.Configure<XiHanObjectStorageOptions>(_ => { });
        }

        services.Configure<XiHanObjectStorageProviderOptions>(_ => { });
        services.TryAddSingleton<IFileStorageProviderManager, DefaultFileStorageProviderManager>();
        services.TryAddSingleton<IFileStorageRouter, DefaultFileStorageRouter>();
        return services;
    }

    /// <summary>
    /// 注册自定义文件存储提供程序
    /// </summary>
    /// <typeparam name="TProvider">提供程序类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="providerName">提供程序名称</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddFileStorageProvider<TProvider>(
        this IServiceCollection services,
        string providerName)
        where TProvider : class, IFileStorageProvider
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

        var normalizedProviderName = providerName.Trim();
        services.TryAddSingleton<TProvider>();
        services.Configure<XiHanObjectStorageProviderOptions>(options =>
        {
            options.AddProvider(normalizedProviderName, typeof(TProvider));
        });

        return services;
    }

    /// <summary>
    /// 注册本地文件存储提供程序
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">本地存储配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddLocalFileStorageProvider(
        this IServiceCollection services,
        Action<LocalStorageOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.Configure<LocalStorageOptions>(_ => { });
        }

        return services.AddFileStorageProvider<LocalFileStorageProvider>(ObjectStorageProviderNames.Local);
    }

    /// <summary>
    /// 注册 S3 兼容文件存储提供程序（Amazon S3、MinIO、阿里云 OSS、腾讯云 COS）
    /// </summary>
    /// <remarks>
    /// 客户端与存储来自 5.0 的 <c>S3ObjectStorageFeature</c>（宿主在配置了 <c>XiHan:ObjectStorage:S3</c> 时装配）；
    /// 没有装配时首次取用会因缺少服务而失败，不会静默落到别的存储。历史名 <c>MinIO</c>、<c>AliyunOSS</c>、<c>TencentCOS</c>
    /// 作为别名指向同一个提供程序，既有文件记录与配置继续可用。
    /// </remarks>
    /// <param name="services">服务集合</param>
    /// <param name="configure">应用级 S3 配置（默认桶、公开地址）</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddS3FileStorageProvider(
        this IServiceCollection services,
        Action<S3StorageOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.Configure<S3StorageOptions>(_ => { });
        }

        services.TryAddSingleton(static sp => new S3FileStorageProvider(
            sp.GetRequiredService<S3BlobStore>(),
            sp.GetRequiredService<IAmazonS3>(),
            sp.GetRequiredService<S3ObjectStorageOptions>(),
            sp.GetRequiredService<IOptions<S3StorageOptions>>().Value));
        services.Configure<XiHanObjectStorageProviderOptions>(static options =>
        {
            options.AddProvider(ObjectStorageProviderNames.S3, typeof(S3FileStorageProvider));
            options.AddProvider(ObjectStorageProviderNames.Minio, typeof(S3FileStorageProvider));
            options.AddProvider(ObjectStorageProviderNames.AliyunOss, typeof(S3FileStorageProvider));
            options.AddProvider(ObjectStorageProviderNames.TencentCos, typeof(S3FileStorageProvider));
        });

        return services;
    }

    /// <summary>
    /// 按配置注册提供程序
    /// </summary>
    private static void RegisterConfiguredProviders(IServiceCollection services, IConfiguration config)
    {
        var storageOptions = config.GetSection(XiHanObjectStorageOptions.SectionName).Get<XiHanObjectStorageOptions>() ?? new XiHanObjectStorageOptions();

        var providerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var providerName in storageOptions.EnabledProviders ?? [])
        {
            if (!string.IsNullOrWhiteSpace(providerName))
            {
                providerNames.Add(providerName.Trim());
            }
        }

        if (!string.IsNullOrWhiteSpace(storageOptions.DefaultProvider))
        {
            providerNames.Add(storageOptions.DefaultProvider.Trim());
        }

        if (providerNames.Count == 0)
        {
            providerNames.Add(ObjectStorageProviderNames.Local);
        }

        foreach (var providerName in providerNames)
        {
            RegisterProvider(services, providerName);
        }
    }

    /// <summary>
    /// 注册单个提供程序
    /// </summary>
    private static void RegisterProvider(IServiceCollection services, string providerName)
    {
        switch (providerName.Trim().ToUpperInvariant())
        {
            case "LOCAL":
                services.AddLocalFileStorageProvider();
                break;

            case "S3":
            case "MINIO":
            case "ALIYUNOSS":
            case "TENCENTCOS":
                services.AddS3FileStorageProvider();
                break;

            default:
                throw new InvalidOperationException($"不支持的对象存储提供程序：{providerName}");
        }
    }
}
