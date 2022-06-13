using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.Implementations;

/// <summary>
/// Generic implementation of entity search, designed to allow for quickly creating new APIs with search capability
/// </summary>
/// <typeparam name="TModel">The model type we will be returning</typeparam>
public class EntitySearch<TModel> : IEntitySearch<TModel> 
    where TModel : class, IModel
{
    private readonly ISearchContext _context;
    private readonly IModelMapper<TModel> _mapper;
    private readonly IRepositoryMetricService _logService;
    private readonly IEnumerable<IEntitySearchFormatter<TModel>> _searchFormatters;
        
    public EntitySearch(ISearchContext context, 
        IModelMapper<TModel> mapper,
        IRepositoryMetricService logService,
        IEnumerable<IEntitySearchFormatter<TModel>> searchFormatters)
    {
        _context = context;
        _mapper = mapper;
        _logService = logService;
        _searchFormatters = searchFormatters;
    }
        
    /// <summary>
    /// Singe function that takes a list of search filters and returns a list of formatted models
    /// </summary>
    /// <param name="entitySearch">The query model sent from the user</param>
    /// <param name="mandatoryFilters">Mandatory filters used for applying server side
    /// filtering such as context sensitive user / entity</param>
    /// <returns>A list of formatted models</returns>
    public async Task<EntitySearchResponse<TModel>> GetSearchResults<TEntity>(EntitySearchQueryModel entitySearch, 
        List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntity
    {
        _logService.StartClock();
        
        // Get search results for our entities
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Querying Results", RepositoryMetricType.Start);
        EntitySearchResult<TEntity> searchResults = await _context
            .SearchEntities<TEntity>(entitySearch, mandatoryFilters);
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Querying Results", RepositoryMetricType.Complete);

        return await _searchToResult(entitySearch, searchResults);
    }

    /// <summary>
    /// Singe function that takes a list of search filters and returns a list of formatted models
    /// </summary>
    /// <param name="baseQuery">The base query to execute, should contain minimal filters as these are applied
    /// based on user input</param>
    /// <param name="entitySearch">The query model sent from the user</param>
    /// <param name="mandatoryFilters">Mandatory filters used for applying server side
    /// filtering such as context sensitive user / entity</param>
    /// <returns>A list of formatted models</returns>
    public async Task<EntitySearchResponse<TModel>> GetSearchResults<TEntity>(IQueryable<TEntity> baseQuery, EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntity
    {
        _logService.StartClock();
        
        // Get search results for our entities
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Querying Results", RepositoryMetricType.Start);
        EntitySearchResult<TEntity> searchResults = await _context
            .SearchEntities(baseQuery, entitySearch, mandatoryFilters);
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Querying Results", RepositoryMetricType.Complete);
        
        return await _searchToResult(entitySearch, searchResults);
    }

    private async Task<EntitySearchResponse<TModel>> _searchToResult<TEntity>(EntitySearchQueryModel entitySearch, EntitySearchResult<TEntity> searchResults)
        where TEntity : class, IEntity
    {
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Calculating pagination", RepositoryMetricType.Start);
        EntitySearchResponse<TModel> pagedResult = new()
        {
            Paging = entitySearch.CalculatePagination(searchResults.RecordCount)
        };
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Calculating pagination", RepositoryMetricType.Complete);

        // Format them into models
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Mapping entities", RepositoryMetricType.Start);
        foreach (TEntity result in searchResults.Records)
        {
            // Create a new model using the mapper
            TModel newModel = _mapper.Create(result);

            // If we have a search formatter we can use it here to populate any additional data
            foreach (IEntitySearchFormatter<TModel> entitySearchFormatter in _searchFormatters)
            {
                newModel = await entitySearchFormatter.FormatModel(newModel);
            }

            // Add to the list
            pagedResult.Results.Add(newModel);
        }
        _logService.LogMetric(nameof(EntitySearch<TModel>),"Mapping entities", RepositoryMetricType.Complete);
        
        return pagedResult;
    }
}