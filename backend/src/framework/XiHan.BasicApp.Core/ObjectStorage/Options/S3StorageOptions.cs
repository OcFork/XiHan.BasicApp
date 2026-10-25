// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.ObjectStorage.Options;

/// <summary>
/// S3 兼容对象存储提供程序的应用级配置
/// </summary>
/// <remarks>
/// 与 5.0 <c>S3ObjectStorageFeature</c> 共用配置节 <c>XiHan:ObjectStorage:S3</c>：连接（<c>ServiceUrl</c>、<c>Region</c>、
/// <c>AccessKey</c>、<c>SecretKey</c>、<c>ForcePathStyle</c>、<c>EnsureBuckets</c>）由框架读取，本类只读应用自己关心的桶与公开地址。
/// </remarks>
public class S3StorageOptions
{
    /// <summary>
    /// 配置节
    /// </summary>
    public const string SectionName = "XiHan:ObjectStorage:S3";

    /// <summary>
    /// 默认存储桶：上传请求没有指定桶时用它
    /// </summary>
    public string DefaultBucket { get; set; } = string.Empty;

    /// <summary>
    /// 默认桶的公开访问地址前缀（CDN 或反向代理）；为空时按端点与桶名拼出对象地址
    /// </summary>
    public string? PublicUrl { get; set; }
}
