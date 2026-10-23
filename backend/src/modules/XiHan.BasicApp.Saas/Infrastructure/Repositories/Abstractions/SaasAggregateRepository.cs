// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.Framework.Domain.Aggregates.Abstracts;

namespace XiHan.BasicApp.Saas.Infrastructure.Repositories;

/// <summary>
/// SaaS SqlSugar 聚合根仓储基类
/// </summary>
/// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
// xihan-migrate(V4-03): SaasAggregateRepository 没法自动注册（泛型、抽象或不在项目里）；请手工注册。
public abstract class SaasAggregateRepository<TAggregateRoot>(
    ISqlSugarClientResolver clientResolver,
    IUnitOfWorkManager unitOfWorkManager)
    : SqlSugarAggregateRepository<TAggregateRoot, long>(clientResolver, unitOfWorkManager), ISaasAggregateRepository<TAggregateRoot>
    where TAggregateRoot : class, IAggregateRoot<long>, new();
