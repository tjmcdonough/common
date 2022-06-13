using Airslip.Common.Repository.Types.Constants;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Services.SqlServer.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Airslip.Common.Services.SqlServer.Implementations;

public class QueryBuilder : IQueryBuilder
{
    public IOrderedQueryable<T> OrderBy<T>(IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    public IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }
    public IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> source, string propertyName)
    {
        return source.ThenBy(ToLambda<T>(propertyName));
    }

    public IOrderedQueryable<T> ThenByDescending<T>(IOrderedQueryable<T> source, string propertyName)
    {
        return source.ThenByDescending(ToLambda<T>(propertyName));
    }

    public Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        MemberExpression property = Expression.Property(parameter, propertyName);
        UnaryExpression propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);            
    }

    public Expression<Func<T, bool>> AndAlso<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        // need to detect whether they use the same
        // parameter instance; if not, they need fixing
        ParameterExpression param = expr1.Parameters[0];
        if (ReferenceEquals(param, expr2.Parameters[0]))
        {
            // simple version
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, expr2.Body), param);
        }
        // otherwise, keep expr1 "as is" and invoke expr2
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                expr1.Body,
                Expression.Invoke(expr2, param)), param);
    }

    public Expression<Func<T, bool>> OrElse<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        // need to detect whether they use the same
        // parameter instance; if not, they need fixing
        ParameterExpression param = expr1.Parameters[0];
        if (ReferenceEquals(param, expr2.Parameters[0]))
        {
            // simple version
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, expr2.Body), param);
        }
        // otherwise, keep expr1 "as is" and invoke expr2
        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                expr1.Body,
                Expression.Invoke(expr2, param)), param);
    }

    public Expression<Func<TEntity, bool>> CreateLambdaExpression<TEntity>(SearchFilterModel filterModel)
    {
        ParameterExpression arg = Expression.Parameter(typeof(TEntity), "p");
        PropertyInfo? property = typeof(TEntity).GetProperty(filterModel.ColumnField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        dynamic compareValue = Expression.Constant(GetTypedValue(filterModel.Value, property!.PropertyType, filterModel.OperatorValue));
        MemberExpression member = Expression.MakeMemberAccess(arg, property);
        
        Expression comparison;
        switch (filterModel.OperatorValue)
        {
            case Operators.OPERATOR_CONTAINS:
                comparison = Expression.Call(
                    member,
                    "Contains",
                    null, 
                    compareValue);
                break;
            case Operators.OPERATOR_EQUALS:
                comparison = Expression.Equal(
                    member,
                    compareValue);
                break;
            case Operators.OPERATOR_GREATER_THAN_EQUALS:
            case Operators.OPERATOR_ON_OR_AFTER:
                comparison = Expression.GreaterThanOrEqual(
                    member,
                    compareValue);
                break;
            case Operators.OPERATOR_LESS_THAN_EQUALS:
            case Operators.OPERATOR_ON_OR_BEFORE:
                comparison = Expression.LessThanOrEqual(member, compareValue);
                break;
            case Operators.OPERATOR_GREATER_THAN:
            case Operators.OPERATOR_AFTER:
                comparison = Expression.GreaterThan(member, compareValue);
                break;
            case Operators.OPERATOR_LESS_THAN:
            case Operators.OPERATOR_BEFORE:
                comparison = Expression.LessThan(member, compareValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return Expression.Lambda<Func<TEntity, bool>>(comparison, arg);
    }

    public dynamic GetTypedValue(dynamic value, Type targetType, string operatorValue)
    {
        if (operatorValue.InList(Operators.OPERATOR_AFTER, Operators.OPERATOR_BEFORE,
                Operators.OPERATOR_ON_OR_AFTER, Operators.OPERATOR_ON_OR_BEFORE))
        {
            string rawDate = Convert.ChangeType(value, typeof(string));
            CultureInfo cultureInfo = new("en-GB");
            DateTime date = DateTime.ParseExact(rawDate, "yyyy-MM-dd", cultureInfo);

            value = targetType == typeof(long) ? date.ToUnixTimeMilliseconds() : date;
        }
        
        return targetType.IsEnum ? 
            Enum.Parse(targetType, (string) value) : 
            Convert.ChangeType(value, targetType);
    }
    
    public IQueryable<TEntity> BuildQuery<TEntity>(DbSet<TEntity> set, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class
    {
        return BuildQuery(set.AsQueryable(), entitySearch, mandatoryFilters);
    }
    
    public IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> q, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class
    {
        Expression<Func<TEntity, bool>>? lambda = BuildFilterQuery<TEntity>(mandatoryFilters);
        if (lambda != null) q = q.Where(lambda);

        if (entitySearch.Search?.Items == null || entitySearch.Search.Items.Count == 0)
            return q;
        
        lambda = BuildFilterQuery<TEntity>(entitySearch.Search.Items, entitySearch.Search.LinkOperator);
        if (lambda != null) q = q.Where(lambda);
        
        return q;
    }

    public Expression<Func<TEntity, bool>>? BuildFilterQuery<TEntity>(List<SearchFilterModel> filters, 
        string linkOperator = Operators.LINK_OPERATOR_AND)
    {
        Expression<Func<TEntity, bool>>? lambda = null;
        Func<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>>? 
            predicate;
        
        switch (linkOperator)
        {
            case Operators.LINK_OPERATOR_AND:
                predicate = AndAlso;
                break;
            case Operators.LINK_OPERATOR_OR:
                predicate = OrElse;
                break;
            default:
                throw new NotImplementedException();
        }

        foreach (SearchFilterModel searchFilterModel in filters.Where(o => o.Value != null))
        {
            Expression<Func<TEntity, bool>> thisExpression = CreateLambdaExpression<TEntity>(searchFilterModel);
            lambda = lambda == null ? 
                thisExpression : 
                predicate(lambda, thisExpression);
        }

        return lambda;
    }
}