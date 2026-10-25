// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.ObjectStorage.Constants;

/// <summary>
/// 对象存储提供程序名称常量
/// </summary>
public static class ObjectStorageProviderNames
{
    /// <summary>
    /// 本地磁盘
    /// </summary>
    public const string Local = "Local";

    /// <summary>
    /// S3 兼容对象存储（Amazon S3、MinIO、阿里云 OSS、腾讯云 COS 等，经 5.0 的 <c>ObjectStorage.S3</c>）
    /// </summary>
    public const string S3 = "S3";

    /// <summary>
    /// MinIO：历史名，现在是 <see cref="S3"/> 的别名（既有文件记录与配置仍可用它）
    /// </summary>
    public const string Minio = "MinIO";

    /// <summary>
    /// 阿里云 OSS：历史名，现在是 <see cref="S3"/> 的别名（走 OSS 的 S3 兼容端点）
    /// </summary>
    public const string AliyunOss = "AliyunOSS";

    /// <summary>
    /// 腾讯云 COS：历史名，现在是 <see cref="S3"/> 的别名（走 COS 的 S3 兼容端点）
    /// </summary>
    public const string TencentCos = "TencentCOS";
}
