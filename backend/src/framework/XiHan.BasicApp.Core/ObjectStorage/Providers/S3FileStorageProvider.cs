// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using XiHan.BasicApp.Core.ObjectStorage.Constants;
using XiHan.BasicApp.Core.ObjectStorage.Models;
using XiHan.BasicApp.Core.ObjectStorage.Options;
using XiHan.Framework.ObjectStorage.Abstractions;
using XiHan.Framework.ObjectStorage.Abstractions.Blobs;
using XiHan.Framework.ObjectStorage.Abstractions.Keys;
using XiHan.Framework.ObjectStorage.Abstractions.Listing;
using XiHan.Framework.ObjectStorage.S3;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Core.ObjectStorage.Providers;

/// <summary>
/// S3 兼容对象存储提供程序：Amazon S3、MinIO、阿里云 OSS、腾讯云 COS 等都经 5.0 的 <c>ObjectStorage.S3</c>（AWS SDK）访问
/// </summary>
/// <remarks>
/// 读写删、探测与列举走 5.0 的 <see cref="IBlobStore"/>（批量删除一次请求、不覆盖用条件写入）；
/// 预签名地址与服务端复制是 S3 协议能力、不在 <see cref="IBlobStore"/> 契约里，直接用 <see cref="IAmazonS3"/>。
/// 桶即容器：上传请求没有指定桶时用 <see cref="S3StorageOptions.DefaultBucket"/>。不支持分片上传（SDK 按对象整体上传，大对象由框架落临时文件取长度）。
/// 由容器装配时（appsettings 的 <c>XiHan:ObjectStorage:S3</c>）复用 <c>S3ObjectStorageFeature</c> 建的客户端；
/// 由落库的存储配置构建时（<see cref="Create"/>）自己持有客户端并在释放时关闭。
/// </remarks>
public sealed class S3FileStorageProvider : FileStorageProviderBase, IDisposable
{
    private readonly IBlobStore _store;
    private readonly IAmazonS3 _client;
    private readonly S3ObjectStorageOptions _connection;
    private readonly S3StorageOptions _settings;
    private readonly bool _ownsClient;

    /// <summary>
    /// 构造函数（客户端由外部持有）
    /// </summary>
    /// <param name="store">5.0 对象存储（S3 实现）</param>
    /// <param name="client">S3 客户端（预签名与复制）</param>
    /// <param name="connection">框架的 S3 连接选项（端点与寻址风格，拼对象地址用）</param>
    /// <param name="settings">应用级选项（默认桶、公开地址）</param>
    public S3FileStorageProvider(IBlobStore store, IAmazonS3 client, S3ObjectStorageOptions connection, S3StorageOptions settings)
        : this(store, client, connection, settings, ownsClient: false)
    {
    }

    private S3FileStorageProvider(IBlobStore store, IAmazonS3 client, S3ObjectStorageOptions connection, S3StorageOptions settings, bool ownsClient)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _ownsClient = ownsClient;
    }

    /// <summary>
    /// 按连接选项自建客户端与存储（落库的 <c>SysStorageConfig</c> 运行期构建凭证时用）；返回的实例持有客户端，释放时关闭
    /// </summary>
    /// <param name="connection">框架的 S3 连接选项</param>
    /// <param name="settings">应用级选项</param>
    /// <returns>提供程序</returns>
    public static S3FileStorageProvider Create(S3ObjectStorageOptions connection, S3StorageOptions settings)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(settings);
        var client = S3ObjectStorageFeature.CreateClient(connection);
        return new S3FileStorageProvider(new S3BlobStore(client, connection, TimeProvider.System), client, connection, settings, ownsClient: true);
    }

    /// <inheritdoc />
    public override string ProviderName => ObjectStorageProviderNames.S3;

    /// <inheritdoc />
    public override async Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default)
    {
        var (bucket, key) = ParsePath(path, bucketName: null);
        var result = await _store.GetAsync(bucket, key, cancellationToken);
        return result.TryGetValue(out var content) ? content.Content : throw ToException(result.Error!, "下载文件失败");
    }

    /// <inheritdoc />
    public override Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(path, bucketName: null, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(string path, string? bucketName, CancellationToken cancellationToken = default)
    {
        var (bucket, key) = ParsePath(path, bucketName);
        var result = await _store.DeleteAsync(bucket, key, cancellationToken);
        if (!result.IsSuccess)
        {
            throw ToException(result.Error!, "删除文件失败");
        }
    }

    /// <inheritdoc />
    public override async Task DeleteManyAsync(IReadOnlyList<string> paths, string? bucketName = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(paths);
        if (paths.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("文件路径列表不能包含空路径。", nameof(paths));
        }

        var distinct = paths.Distinct(StringComparer.Ordinal).ToList();
        if (distinct.Count == 0)
        {
            return;
        }

        // 一次批量删除：框架逐项给出结果，失败的汇总成一个 AggregateException（与接口约定一致）
        var bucket = ResolveBucket(bucketName);
        var results = await _store.DeleteAsync(bucket, [.. distinct.Select(path => new BlobKey(NormalizePath(path)))], cancellationToken);
        var failures = results
            .Where(static result => !result.Outcome.IsSuccess)
            .Select(result => (Exception)new InvalidOperationException($"删除文件 '{result.Key.Value}' 失败：{result.Outcome.Error!.Message}"))
            .ToList();
        if (failures.Count > 0)
        {
            throw new AggregateException($"{failures.Count} 个文件删除失败。", failures);
        }
    }

    /// <inheritdoc />
    public override Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        return ExistsAsync(path, bucketName: null, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<bool> ExistsAsync(string path, string? bucketName, CancellationToken cancellationToken = default)
    {
        var (bucket, key) = ParsePath(path, bucketName);
        var result = await _store.HeadAsync(bucket, key, cancellationToken);
        if (result.IsSuccess)
        {
            return true;
        }

        // 不存在是正常答案；存储不可达要报出来，不能伪装成"不存在"
        return result.Error!.Code == ObjectStorageErrors.BlobNotFound ? false : throw ToException(result.Error, "探测文件失败");
    }

    /// <inheritdoc />
    public override Task<FileMetadata> GetMetadataAsync(string path, CancellationToken cancellationToken = default)
    {
        return GetMetadataAsync(path, bucketName: null, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<FileMetadata> GetMetadataAsync(string path, string? bucketName, CancellationToken cancellationToken = default)
    {
        var (bucket, key) = ParsePath(path, bucketName);
        var result = await _store.HeadAsync(bucket, key, cancellationToken);
        return result.TryGetValue(out var metadata) ? ToFileMetadata(bucket, metadata, path) : throw ToException(result.Error!, "获取文件元数据失败");
    }

    /// <inheritdoc />
    public override async Task<string> GeneratePresignedUrlAsync(string path, TimeSpan expiresIn, CancellationToken cancellationToken = default)
    {
        var (bucket, key) = ParsePath(path, bucketName: null);
        try
        {
            return await _client.GetPreSignedURLAsync(new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = key.Value,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.Add(expiresIn),
            });
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new InvalidOperationException($"生成预签名URL失败: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public override async Task CopyAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
    {
        var (sourceBucket, sourceKey) = ParsePath(sourcePath, bucketName: null);
        var (destinationBucket, destinationKey) = ParsePath(destinationPath, bucketName: null);
        try
        {
            // 服务端复制：不把内容拉回本地再传一遍
            _ = await _client.CopyObjectAsync(new CopyObjectRequest
            {
                SourceBucket = sourceBucket,
                SourceKey = sourceKey.Value,
                DestinationBucket = destinationBucket,
                DestinationKey = destinationKey.Value,
            }, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new InvalidOperationException($"复制文件失败: {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public override async Task MoveAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
    {
        await CopyAsync(sourcePath, destinationPath, cancellationToken);
        await DeleteAsync(sourcePath, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<List<FileMetadata>> ListFilesAsync(string path, bool recursive = false, CancellationToken cancellationToken = default)
    {
        var bucket = ResolveBucket(null);
        var prefix = NormalizePath(path ?? string.Empty);
        var files = new List<FileMetadata>();
        string? token = null;
        do
        {
            var page = await _store.ListAsync(bucket, new BlobQuery { Prefix = prefix, ContinuationToken = token }, cancellationToken);
            if (!page.TryGetValue(out var items))
            {
                throw ToException(page.Error!, "列出文件失败");
            }

            foreach (var item in items.Items)
            {
                // S3 没有目录：不递归就只要前缀之后不再有分隔符的键
                if (recursive || !item.Key.Value.AsSpan(prefix.Length).Contains('/'))
                {
                    files.Add(ToFileMetadata(bucket, item, item.Key.Value));
                }
            }

            token = items.ContinuationToken;
        }
        while (token is not null);

        return files;
    }

    /// <inheritdoc />
    protected override async Task<FileUploadResult> UploadCoreAsync(FileUploadRequest request, CancellationToken cancellationToken)
    {
        var bucket = ResolveBucket(request.BucketName);
        var key = new BlobKey(NormalizePath(request.StoragePath));
        var options = new BlobPutOptions
        {
            ContentType = string.IsNullOrWhiteSpace(request.ContentType) ? BlobPutOptions.DefaultContentType : request.ContentType,
            Metadata = NormalizeMetadata(request.Metadata),
            Overwrite = request.Overwrite,
        };
        var result = await _store.PutAsync(bucket, key, request.FileStream, options, cancellationToken);
        if (!result.TryGetValue(out var metadata))
        {
            return new FileUploadResult { Success = false, ErrorMessage = $"S3上传失败: {result.Error!.Message}" };
        }

        request.ProgressCallback?.Invoke(metadata.Length, metadata.Length);
        return new FileUploadResult
        {
            Success = true,
            Path = request.StoragePath,
            Url = BuildUrl(bucket, key.Value),
            FileSize = metadata.Length,
            ETag = metadata.ETag,
        };
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsClient)
        {
            _client.Dispose();
        }
    }

    #region 私有方法

    private (string Bucket, BlobKey Key) ParsePath(string path, string? bucketName)
    {
        return (ResolveBucket(bucketName), new BlobKey(NormalizePath(path)));
    }

    private string ResolveBucket(string? bucketName)
    {
        if (!string.IsNullOrWhiteSpace(bucketName))
        {
            return bucketName.Trim();
        }

        return string.IsNullOrWhiteSpace(_settings.DefaultBucket)
            ? throw new InvalidOperationException("S3 对象存储未配置默认存储桶（XiHan:ObjectStorage:S3:DefaultBucket）。")
            : _settings.DefaultBucket.Trim();
    }

    private FileMetadata ToFileMetadata(string bucket, BlobMetadata metadata, string path)
    {
        return new FileMetadata
        {
            Name = GetObjectName(metadata.Key.Value),
            Path = path,
            Size = metadata.Length,
            ContentType = metadata.ContentType,
            LastModified = metadata.LastModified == DateTimeOffset.MinValue ? null : metadata.LastModified,
            ETag = metadata.ETag,
            IsDirectory = false,
            Url = BuildUrl(bucket, metadata.Key.Value),
            Metadata = metadata.Metadata.Count == 0 ? null : new Dictionary<string, string>(metadata.Metadata, StringComparer.Ordinal),
        };
    }

    /// <summary>
    /// 对象地址：默认桶配了公开地址用它；否则按端点与寻址风格拼（路径式 <c>端点/桶/键</c>，虚拟主机式 <c>桶.端点/键</c>；无端点按 Amazon S3 区域地址）
    /// </summary>
    private string BuildUrl(string bucket, string key)
    {
        if (!string.IsNullOrWhiteSpace(_settings.PublicUrl) && string.Equals(bucket, _settings.DefaultBucket.Trim(), StringComparison.Ordinal))
        {
            return $"{_settings.PublicUrl.TrimEnd('/')}/{key}";
        }

        if (_connection.ServiceUrl is { } serviceUrl)
        {
            var uri = new Uri(serviceUrl);
            return _connection.ForcePathStyle
                ? $"{serviceUrl.TrimEnd('/')}/{bucket}/{key}"
                : $"{uri.Scheme}://{bucket}.{uri.Authority}/{key}";
        }

        return $"https://{bucket}.s3.{_connection.Region}.amazonaws.com/{key}";
    }

    /// <summary>
    /// 自定义元数据名按 S3 的限制规整：小写、只留字母数字与连字符、至多 64 个字符；规整后为空的项丢弃
    /// </summary>
    private static Dictionary<string, string> NormalizeMetadata(Dictionary<string, string>? metadata)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (metadata is null)
        {
            return result;
        }

        foreach (var (name, value) in metadata)
        {
            var builder = new StringBuilder(name.Length);
            foreach (var c in name.ToLowerInvariant())
            {
                builder.Append(c is >= 'a' and <= 'z' or >= '0' and <= '9' or '-' ? c : '-');
            }

            var normalized = builder.ToString().TrimStart('-');
            if (normalized.Length == 0)
            {
                continue;
            }

            result[normalized.Length > 64 ? normalized[..64] : normalized] = value ?? string.Empty;
        }

        return result;
    }

    private static Exception ToException(Error error, string action)
    {
        return error.Code == ObjectStorageErrors.BlobNotFound
            ? new FileNotFoundException($"{action}: {error.Message}")
            : new InvalidOperationException($"{action}: {error.Message}");
    }

    #endregion
}
