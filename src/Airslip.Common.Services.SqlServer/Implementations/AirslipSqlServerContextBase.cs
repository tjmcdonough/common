using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Airslip.Common.Services.SqlServer.Interfaces;

namespace Airslip.Common.Services.SqlServer.Implementations;

public abstract class AirslipSqlServerContextBase : DbContext, ISearchContext, IContext
{
    private readonly IRepositoryMetricService _metricService;
    protected readonly IQueryBuilder QueryBuilder;

    protected AirslipSqlServerContextBase(DbContextOptions options, IRepositoryMetricService metricService, 
        IQueryBuilder queryBuilder)
        : base(options)
    {
        _metricService = metricService;
        QueryBuilder = queryBuilder;
    }

    public virtual async Task<TEntity> AddEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(AddEntity), RepositoryMetricType.Start);
        EntityEntry<TEntity> result = await Set<TEntity>().AddAsync(newEntity);
        await SaveChangesAsync();
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(AddEntity), RepositoryMetricType.Complete);
        return result.Entity;
    }

    public virtual async Task<TEntity?> GetEntity<TEntity>(string id) where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(GetEntity), RepositoryMetricType.Start);
        TEntity? result = await Set<TEntity>().FindAsync(id);
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(GetEntity), RepositoryMetricType.Complete);
        return result;
    }

    public virtual async Task<TEntity> UpdateEntity<TEntity>(TEntity updatedEntity) where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(UpdateEntity), RepositoryMetricType.Start);
        EntityEntry<TEntity> updateResult = Update(updatedEntity);
        await SaveChangesAsync();
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(UpdateEntity), RepositoryMetricType.Complete);
        return updateResult.Entity;
    }

    public virtual IQueryable<TEntity> QueryableOf<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
    }

    public virtual async Task<TEntity> UpsertEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(UpsertEntity), RepositoryMetricType.Start);
        IQueryable<string> q = from table in Set<TEntity>().AsQueryable()
            where table.Id == newEntity.Id
            select table.Id;

        TEntity result;
        
        if (!await q.AnyAsync())
        {
            result =  await AddEntity(newEntity);
        }
        else
        {
            result =  await UpdateEntity(newEntity);
        }
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(UpsertEntity), RepositoryMetricType.Complete);
        
        return result;
    }

    public virtual async Task<TEntity> Update<TEntity>(string id, string field, string value) where TEntity : class, IEntityWithId
    {
        TEntity? result = await Set<TEntity>().FindAsync(id);

        if (result == null) return result!;
        
        _set(result, field, value);
        await SaveChangesAsync();

        return result;
    }
    
    private static void _set<T, TProperty>(T instance, string propertyName, TProperty value)
    {
        ParameterExpression instanceExpression = Expression.Parameter(typeof(T), "p");
        MemberExpression propertyGetterExpression = Expression.PropertyOrField(instanceExpression, propertyName);
        ParameterExpression newValueExpression = Expression.Parameter(typeof(TProperty), "value");
        BinaryExpression assignmentExpression = Expression.Assign(propertyGetterExpression, newValueExpression);
        Expression<Action<T, TProperty>> lambdaExpression = Expression.Lambda<Action<T, TProperty>>(assignmentExpression, instanceExpression, newValueExpression);
        Action<T, TProperty> setter = lambdaExpression.Compile();
        setter(instance, value);
    }

    public virtual async Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) 
        where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(SearchEntities), RepositoryMetricType.Start);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryBuilder.BuildQuery), RepositoryMetricType.Start);
        IQueryable<TEntity> query = QueryBuilder.BuildQuery(Set<TEntity>(), entitySearch, mandatoryFilters);
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryBuilder.BuildQuery), RepositoryMetricType.Complete);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryToSearchResult), RepositoryMetricType.Start);
        EntitySearchResult<TEntity> result = await QueryToSearchResult(query, entitySearch);;
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryToSearchResult), RepositoryMetricType.Complete);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(SearchEntities), RepositoryMetricType.Complete);
        return result;
    }

    public virtual async Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(IQueryable<TEntity> baseQuery, 
        EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(SearchEntities), RepositoryMetricType.Start);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryBuilder.BuildQuery), RepositoryMetricType.Start);
        IQueryable<TEntity> query = QueryBuilder.BuildQuery(baseQuery, entitySearch, mandatoryFilters);
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryBuilder.BuildQuery), RepositoryMetricType.Complete);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryToSearchResult), RepositoryMetricType.Start);
        EntitySearchResult<TEntity> result = await QueryToSearchResult(query, entitySearch);;
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(QueryToSearchResult), RepositoryMetricType.Complete);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), nameof(SearchEntities), RepositoryMetricType.Complete);
        
        return result;
    }

    protected virtual async Task<EntitySearchResult<TEntity>> QueryToSearchResult<TEntity>(IQueryable<TEntity> query, 
        EntitySearchQueryModel entitySearch)
        where TEntity : class, IEntityWithId
    {
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), "Record Count", RepositoryMetricType.Start);
        int count = await query.CountAsync();
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), "Record Count", RepositoryMetricType.Complete);

        IOrderedQueryable<TEntity>? orderBy = null;
        foreach (EntitySearchSortModel sortModel in entitySearch.Sort)
        {
            if (orderBy == null)
                orderBy = sortModel.Sort == SortOrder.Asc
                    ? QueryBuilder.OrderBy(query, sortModel.Field)
                    : QueryBuilder.OrderByDescending(query, sortModel.Field);
            else
                orderBy = sortModel.Sort == SortOrder.Asc
                    ? QueryBuilder.ThenBy(orderBy, sortModel.Field)
                    : QueryBuilder.ThenByDescending(orderBy, sortModel.Field);
        }
        query = orderBy ?? query;
        
        if (entitySearch.Page > 0)
            query = query.Skip(entitySearch.Page * entitySearch.RecordsPerPage);
        
        if (entitySearch.RecordsPerPage > 0)
            query = query.Take(entitySearch.RecordsPerPage);
        
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), "Running Query", RepositoryMetricType.Start);
        List<TEntity> list = await query
            .ToListAsync();
        _metricService.LogMetric(nameof(AirslipSqlServerContextBase), "Running Query", RepositoryMetricType.Complete);
        
        return new EntitySearchResult<TEntity>(list, count);
    }

    public virtual async Task<int> RecordCount<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
    {
        return await QueryBuilder.BuildQuery(Set<TEntity>(), entitySearch,  mandatoryFilters)
            .CountAsync();
    }
}