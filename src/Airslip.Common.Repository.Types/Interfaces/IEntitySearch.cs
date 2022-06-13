using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Types.Interfaces;

/// <summary>
/// A generic search definition for use when searching entities
/// </summary>
/// <typeparam name="TModel">The model type we are returning</typeparam>
public interface IEntitySearch<TModel>
    where TModel : class, IModel
{
    /// <summary>
    /// A query to get search results based on a set of search filters
    /// </summary>
    /// <typeparam name="TEntity">The entity type we are searching</typeparam>
    /// <param name="entitySearch">The search query model</param>
    /// <param name="mandatoryFilters">Mandatory filters for defining data ownership</param>
    /// <returns>A list of formatted models</returns>
    Task<EntitySearchResponse<TModel>> GetSearchResults<TEntity>(EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters)
        where TEntity : class, IEntity;

    /// <summary>
    /// A query to get search results based on a set of search filters
    /// </summary>
    /// <typeparam name="TEntity">The entity type we are searching</typeparam>
    /// <param name="baseQuery">The base query to execute, should contain minimal filters as these are applied
    /// based on user input</param>
    /// <param name="entitySearch">The search query model</param>
    /// <param name="mandatoryFilters">Mandatory filters for defining data ownership</param>
    /// <returns>A list of formatted models</returns>
    Task<EntitySearchResponse<TModel>> GetSearchResults<TEntity>(IQueryable<TEntity> baseQuery, EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters)
        where TEntity : class, IEntity;
}