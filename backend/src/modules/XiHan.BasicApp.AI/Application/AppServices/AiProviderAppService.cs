// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.AI.Application.Contracts;
using XiHan.BasicApp.AI.Application.Dtos;
using XiHan.BasicApp.AI.Application.Mappers;
using XiHan.BasicApp.AI.Domain.DomainServices;
using XiHan.BasicApp.AI.Domain.Entities;
using XiHan.BasicApp.AI.Domain.Permissions;
using XiHan.BasicApp.Saas.Application.Services;

namespace XiHan.BasicApp.AI.Application.AppServices;

/// <summary>
/// AI Provider 命令应用服务
/// </summary>
public sealed class AiProviderAppService : AiApplicationService, IAiProviderAppService
{
    private readonly IAiProviderDomainService _providerDomainService;

    /// <summary>
    /// 框架会话解析器（写入后使已缓存 IChatClient 失效，配置热切换）
    /// </summary>
    private readonly IAiChatClientResolver _chatClientResolver;

    /// <summary>
    /// 框架嵌入解析器（写入后使已缓存 IEmbeddingGenerator 失效，RAG 配置热切换）
    /// </summary>
    private readonly IAiEmbeddingGeneratorResolver _embeddingResolver;

    private readonly IFieldSecurityService _fieldSecurity;

    /// <summary>
    /// 构造函数
    /// </summary>
    public AiProviderAppService(
        IAiProviderDomainService providerDomainService,
        IAiChatClientResolver chatClientResolver,
        IAiEmbeddingGeneratorResolver embeddingResolver,
        IFieldSecurityService fieldSecurity)
    {
        _providerDomainService = providerDomainService;
        _chatClientResolver = chatClientResolver;
        _embeddingResolver = embeddingResolver;
        _fieldSecurity = fieldSecurity;
    }

    /// <summary>
    /// 使会话与嵌入解析器缓存一并失效（provider 配置改动后热切换）
    /// </summary>
    private void InvalidateResolvers()
    {
        _chatClientResolver.Invalidate();
        _embeddingResolver.Invalidate();
    }

    /// <summary>
    /// 创建 provider
    /// </summary>
    public async Task<AiProviderDetailDto> CreateAsync(AiProviderCreateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能填写
        await _fieldSecurity.EnsureCreatableAsync(typeof(SysAiProvider), input, cancellationToken);

        var result = await _providerDomainService.CreateProviderAsync(AiProviderApplicationMapper.ToCreateCommand(input), cancellationToken);
        InvalidateResolvers();
        return AiProviderApplicationMapper.ToDetailDto(result.Provider);
    }

    /// <summary>
    /// 更新 provider
    /// </summary>
    public async Task<AiProviderDetailDto> UpdateAsync(AiProviderUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能改，表单交回的脱敏值还原为原值
        await _fieldSecurity.EnsureUpdatableAsync(typeof(SysAiProvider), input.Id, input, cancellationToken);

        var result = await _providerDomainService.UpdateProviderAsync(AiProviderApplicationMapper.ToUpdateCommand(input), cancellationToken);
        InvalidateResolvers();
        return AiProviderApplicationMapper.ToDetailDto(result.Provider);
    }

    /// <summary>
    /// 更新 provider 状态
    /// </summary>
    public async Task<AiProviderDetailDto> UpdateStatusAsync(AiProviderStatusUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能改，表单交回的脱敏值还原为原值
        await _fieldSecurity.EnsureUpdatableAsync(typeof(SysAiProvider), input.Id, input, cancellationToken);

        var result = await _providerDomainService.UpdateProviderStatusAsync(AiProviderApplicationMapper.ToStatusCommand(input), cancellationToken);
        InvalidateResolvers();
        return AiProviderApplicationMapper.ToDetailDto(result.Provider);
    }

    /// <summary>
    /// 设为默认 provider
    /// </summary>
    public async Task<AiProviderDetailDto> SetDefaultAsync(AiProviderActionDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _providerDomainService.SetDefaultAsync(input.Id, cancellationToken);
        InvalidateResolvers();
        return AiProviderApplicationMapper.ToDetailDto(result.Provider);
    }

    /// <summary>
    /// 删除 provider
    /// </summary>
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _providerDomainService.DeleteProviderAsync(id, cancellationToken);
        InvalidateResolvers();
    }

    /// <summary>
    /// 测试连接（对指定 provider 发起一次极简推理）
    /// </summary>
    public async Task<AiProviderTestConnectionResultDto> TestConnectionAsync(AiProviderActionDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _providerDomainService.TestConnectionAsync(input.Id, cancellationToken);
        return AiProviderApplicationMapper.ToTestResultDto(result);
    }
}
