// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq.Expressions;
using XiHan.BasicApp.Core.Data.SqlSugar.Repository;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Entities;
using XiHan.BasicApp.Saas.Domain.Repositories;

namespace XiHan.BasicApp.Saas.Infrastructure.Repositories;

/// <summary>
/// SaaS SqlSugar 普通实体仓储基类
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
// xihan-migrate(V4-03): SaasRepository 没法自动注册（泛型、抽象或不在项目里）；请手工注册。
public abstract class SaasRepository<TEntity>(ISqlSugarClientResolver clientResolver)
    : SqlSugarRepositoryBase<TEntity, long>(clientResolver), ISaasRepository<TEntity>
    where TEntity : class, IEntityBase<long>, new()
{
    /// <summary>
    /// 跨租户判断是否存在满足条件的行
    /// </summary>
    public async Task<bool> AnyIgnoreTenantAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        cancellationToken.ThrowIfCancellationRequested();

        return await CreateNoTenantQueryable().Where(predicate).AnyAsync(cancellationToken);
    }
}
