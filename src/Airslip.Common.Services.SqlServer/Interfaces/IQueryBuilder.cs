using Airslip.Common.Repository.Types.Constants;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Airslip.Common.Services.SqlServer.Interfaces;

public interface IQueryBuilder
{
    IOrderedQueryable<T> OrderBy<T>(IQueryable<T> source, string propertyName);
    IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> source, string propertyName);
    Expression<Func<T, object>> ToLambda<T>(string propertyName);

    Expression<Func<T, bool>> AndAlso<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2);

    Expression<Func<T, bool>> OrElse<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2);

    Expression<Func<TEntity, bool>> CreateLambdaExpression<TEntity>(SearchFilterModel filterModel);
    dynamic GetTypedValue(dynamic value, Type targetType, string operatorValue);

    IQueryable<TEntity> BuildQuery<TEntity>(DbSet<TEntity> set, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class;

    IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> q, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class;

    Expression<Func<TEntity, bool>>? BuildFilterQuery<TEntity>(List<SearchFilterModel> filters, 
        string linkOperator = Operators.LINK_OPERATOR_AND);
    IOrderedQueryable<TEntity> ThenBy<TEntity>(IOrderedQueryable<TEntity> source, string propertyName);
    IOrderedQueryable<TEntity> ThenByDescending<TEntity>(IOrderedQueryable<TEntity> source, string propertyName);
}