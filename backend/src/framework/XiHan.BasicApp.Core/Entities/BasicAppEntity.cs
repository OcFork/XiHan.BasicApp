// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;
using XiHan.BasicApp.Core.Entities;
using XiHan.BasicApp.Core.Tenancy;
using XiHan.Framework.Domain;

namespace XiHan.BasicApp.Core.Entities;

/// <summary>
/// BasicApp 实体基类
/// </summary>
/// <remarks>
/// 本项目统一约定：
/// - 平台级/全局记录统一使用 TenantId = 0（"平台租户"占位），不得使用 NULL
/// - 业务租户 Id 从 1 开始分配；0 号租户由平台保留
/// - 全局/平台记录的判定统一以 TenantId == 0 为准；如需 IsGlobal 语义，请在实体 Expand 中以派生只读属性 IsGlobal => TenantId == 0 暴露，不再落库（避免与 TenantId 漂移）
/// - 查询合并全局 + 私有：WHERE TenantId IN (0, {currentTenantId})
/// - 服务层应有 ITenantContext 自动注入 TenantId，禁止业务代码直接操纵
///
/// 【审计三件套索引规范】
/// - 所有具体实体必须包含：IX_{table}_TeId_CrTi、IX_{table}_CrId
/// - FullAudited/AggregateRoot 实体额外包含：IX_{table}_TeId_IsDe
/// - 日志类分表实体将 {table} 替换为 {split_table}
///
/// 【软删与唯一索引约定】
/// - FullAudited/AggregateRoot 等支持软删（IsDeleted）的实体，其唯一索引（UX_*）末列统一附加 IsDeleted，
///   使唯一性仅约束未删除行（IsDeleted=false），从而软删后可再次创建同编码记录
/// - 限制：同一编码至多保留一条软删行（IsDeleted=true）；如需第二次软删同编码记录，服务层须先物理清理旧软删行
/// - 纯创建型（Creation）关联/日志实体为硬删，无 IsDeleted，唯一索引保持原样
/// </remarks>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppEntity : IEntity<long>, IHasConcurrencyStamp, IMultiTenant, IEntityBase<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;

    /// <summary>
    /// 构造函数
    /// </summary>
    protected BasicAppEntity()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="basicId"></param>
    protected BasicAppEntity(long basicId)
    {
        Id = basicId;
    }
}

/// <summary>
/// BasicApp 实体基类（自增主键）
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppEntityWithIdentity : IEntity<long>, IHasConcurrencyStamp, IMultiTenant, IEntityBase<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;
}

/// <summary>
/// BasicApp 创建审计实体基类
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppCreationEntity : IEntity<long>, IHasConcurrencyStamp, IHasCreationTime, IMultiTenant, IEntityBase<long>, ICreationEntity<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Created_Time", IsNullable = false, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间")]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>创建者标识。</summary>
    [SugarColumn(ColumnName = "Created_Id", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建者唯一标识")]
    public virtual long? CreatedId { get; set; }

    /// <summary>创建人。</summary>
    [SugarColumn(ColumnName = "Created_By", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人")]
    public virtual string? CreatedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;
}

/// <summary>
/// BasicApp 修改审计实体基类
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppModificationEntity : IEntity<long>, IHasConcurrencyStamp, IHasCreationTime, IHasModificationTime, IMultiTenant, IEntityBase<long>, ICreationEntity<long>, IModificationEntity<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Created_Time", IsNullable = false, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间")]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>创建者标识。</summary>
    [SugarColumn(ColumnName = "Created_Id", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建者唯一标识")]
    public virtual long? CreatedId { get; set; }

    /// <summary>创建人。</summary>
    [SugarColumn(ColumnName = "Created_By", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人")]
    public virtual string? CreatedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Modified_Time", IsNullable = true, ColumnDescription = "修改时间")]
    public virtual DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>修改者标识。</summary>
    [SugarColumn(ColumnName = "Modified_Id", IsNullable = true, ColumnDescription = "修改者唯一标识")]
    public virtual long? ModifiedId { get; set; }

    /// <summary>修改人。</summary>
    [SugarColumn(ColumnName = "Modified_By", IsNullable = true, ColumnDescription = "修改人")]
    public virtual string? ModifiedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;
}

/// <summary>
/// BasicApp 删除审计实体基类
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppDeletionEntity : IEntity<long>, IHasConcurrencyStamp, IHasCreationTime, IHasModificationTime, ISoftDeletable, IMultiTenant, IFullAuditedEntity<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Created_Time", IsNullable = false, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间")]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>创建者标识。</summary>
    [SugarColumn(ColumnName = "Created_Id", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建者唯一标识")]
    public virtual long? CreatedId { get; set; }

    /// <summary>创建人。</summary>
    [SugarColumn(ColumnName = "Created_By", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人")]
    public virtual string? CreatedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Modified_Time", IsNullable = true, ColumnDescription = "修改时间")]
    public virtual DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>修改者标识。</summary>
    [SugarColumn(ColumnName = "Modified_Id", IsNullable = true, ColumnDescription = "修改者唯一标识")]
    public virtual long? ModifiedId { get; set; }

    /// <summary>修改人。</summary>
    [SugarColumn(ColumnName = "Modified_By", IsNullable = true, ColumnDescription = "修改人")]
    public virtual string? ModifiedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Is_Deleted", IsNullable = false, ColumnDescription = "软删除标记")]
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Deleted_Time", IsNullable = true, ColumnDescription = "删除时间")]
    public virtual DateTimeOffset? DeletedAt { get; set; }

    /// <summary>删除者标识。</summary>
    [SugarColumn(ColumnName = "Deleted_Id", IsNullable = true, ColumnDescription = "删除者唯一标识")]
    public virtual long? DeletedId { get; set; }

    /// <summary>删除人。</summary>
    [SugarColumn(ColumnName = "Deleted_By", IsNullable = true, ColumnDescription = "删除者")]
    public virtual string? DeletedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;
}

/// <summary>
/// BasicApp 完整审计实体基类
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppFullAuditedEntity : IEntity<long>, IHasConcurrencyStamp, IHasCreationTime, IHasModificationTime, ISoftDeletable, IMultiTenant, IFullAuditedEntity<long>, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Created_Time", IsNullable = false, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间")]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>创建者标识。</summary>
    [SugarColumn(ColumnName = "Created_Id", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建者唯一标识")]
    public virtual long? CreatedId { get; set; }

    /// <summary>创建人。</summary>
    [SugarColumn(ColumnName = "Created_By", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人")]
    public virtual string? CreatedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Modified_Time", IsNullable = true, ColumnDescription = "修改时间")]
    public virtual DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>修改者标识。</summary>
    [SugarColumn(ColumnName = "Modified_Id", IsNullable = true, ColumnDescription = "修改者唯一标识")]
    public virtual long? ModifiedId { get; set; }

    /// <summary>修改人。</summary>
    [SugarColumn(ColumnName = "Modified_By", IsNullable = true, ColumnDescription = "修改人")]
    public virtual string? ModifiedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Is_Deleted", IsNullable = false, ColumnDescription = "软删除标记")]
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Deleted_Time", IsNullable = true, ColumnDescription = "删除时间")]
    public virtual DateTimeOffset? DeletedAt { get; set; }

    /// <summary>删除者标识。</summary>
    [SugarColumn(ColumnName = "Deleted_Id", IsNullable = true, ColumnDescription = "删除者唯一标识")]
    public virtual long? DeletedId { get; set; }

    /// <summary>删除人。</summary>
    [SugarColumn(ColumnName = "Deleted_By", IsNullable = true, ColumnDescription = "删除者")]
    public virtual string? DeletedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;

    /// <summary>
    /// 构造函数
    /// </summary>
    protected BasicAppFullAuditedEntity()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="basicId"></param>
    protected BasicAppFullAuditedEntity(long basicId)
    {
        Id = basicId;
    }
}

/// <summary>
/// BasicApp 聚合根基类
/// </summary>
// xihan-migrate(V4-02): ConcurrencyStamp（long）改为 ConcurrencyStamp（string）；把 Row_Version 列改成字符串类型。
// xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
public abstract class BasicAppAggregateRoot : XiHan.BasicApp.Core.Aggregates.IAggregateRoot<long>, IHasConcurrencyStamp, IHasCreationTime, IHasModificationTime, ISoftDeletable, IMultiTenant, IMultiTenantEntity
{
    /// <inheritdoc />
    [SugarColumn(ColumnName = "Basic_Id", IsPrimaryKey = true, IsIdentity = false, ColumnDescription = "主键")]
    public virtual long Id { get; set; } = default!;

    /// <summary>
    /// 是否未持久化：主键仍为默认值
    /// </summary>
    public virtual bool IsTransient() => Id == 0;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Row_Version", Length = 32, ColumnDescription = "版本控制标识，用于处理并发")]
    public virtual string ConcurrencyStamp { get; set; } = string.Empty;

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Created_Time", IsNullable = false, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建时间")]
    public virtual DateTimeOffset CreatedAt { get; set; }

    /// <summary>创建者标识。</summary>
    [SugarColumn(ColumnName = "Created_Id", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建者唯一标识")]
    public virtual long? CreatedId { get; set; }

    /// <summary>创建人。</summary>
    [SugarColumn(ColumnName = "Created_By", IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "创建人")]
    public virtual string? CreatedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Modified_Time", IsNullable = true, ColumnDescription = "修改时间")]
    public virtual DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>修改者标识。</summary>
    [SugarColumn(ColumnName = "Modified_Id", IsNullable = true, ColumnDescription = "修改者唯一标识")]
    public virtual long? ModifiedId { get; set; }

    /// <summary>修改人。</summary>
    [SugarColumn(ColumnName = "Modified_By", IsNullable = true, ColumnDescription = "修改人")]
    public virtual string? ModifiedBy { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Is_Deleted", IsNullable = false, ColumnDescription = "软删除标记")]
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Deleted_Time", IsNullable = true, ColumnDescription = "删除时间")]
    public virtual DateTimeOffset? DeletedAt { get; set; }

    /// <summary>删除者标识。</summary>
    [SugarColumn(ColumnName = "Deleted_Id", IsNullable = true, ColumnDescription = "删除者唯一标识")]
    public virtual long? DeletedId { get; set; }

    /// <summary>删除人。</summary>
    [SugarColumn(ColumnName = "Deleted_By", IsNullable = true, ColumnDescription = "删除者")]
    public virtual string? DeletedBy { get; set; }

    /// <summary>领域事件缓冲；不映射到列。</summary>
    private readonly DomainEventBuffer _events = new();

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> DomainEvents => _events.Items;

    /// <summary>记录领域事件。</summary>
    public void Raise(IDomainEvent domainEvent) => _events.Add(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvents() => _events.Clear();

    /// <inheritdoc />
    [SugarColumn(ColumnName = "Tenant_Id", IsOnlyIgnoreUpdate = true, ColumnDescription = "租户ID")]
    public virtual string TenantId { get; set; } = TenantIds.Platform;

    /// <summary>
    /// 构造函数
    /// </summary>
    protected BasicAppAggregateRoot()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="basicId"></param>
    protected BasicAppAggregateRoot(long basicId)
    {
        Id = basicId;
    }
}
