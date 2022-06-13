using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Services.CosmosDb.Configuration;
using Airslip.Common.Services.CosmosDb.Extensions;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Pluralize.NET.Core;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    public abstract class AirslipCosmosDbSearchBase : ISearchContext
    {
        private readonly IUserContext _userContext;
        protected readonly Database Database;

        protected AirslipCosmosDbSearchBase(CosmosClient cosmosClient, IUserContext userContext,  
            IOptions<CosmosDbSettings> options)
        {
            _userContext = userContext;
            Database = cosmosClient.GetDatabase(options.Value.DatabaseName);
        }
        
        public async Task<List<TEntity>> SearchEntities<TEntity>(List<SearchFilterModel> searchFilters) 
            where TEntity : class, IEntityWithId
        {
            
            if (typeof(IEntityWithOwnership).IsAssignableFrom(typeof(TEntity)))
            {
                switch (_userContext.AirslipUserType ?? AirslipUserType.Standard)
                {
                    case AirslipUserType.Standard:
                        searchFilters.Add(new SearchFilterModel("userId", _userContext.UserId!));
                        break;
                    default:
                        searchFilters.Add(new SearchFilterModel("entityId", 
                            _userContext.EntityId!));
                        searchFilters.Add(new SearchFilterModel("airslipUserType", 
                            _userContext.AirslipUserType!));
                        break;
                } 
            }
            
            Container container = Database.GetContainerForEntity<TEntity>();
            StringBuilder sb = new();
            sb.Append($"SELECT * FROM {AirslipCosmosDbBase.GetContainerId<TEntity>()} f WHERE 1=1");

            foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                sb.Append($" AND f.{searchFilterModel.ColumnField.ToCamelCase()} = @{searchFilterModel.ColumnField}");
            }

            QueryDefinition query = new(sb.ToString());
           foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                query = query.WithParameter($"@{searchFilterModel.ColumnField}", searchFilterModel.ColumnField);
            }    
                
            using FeedIterator<TEntity> feedIterator = container.GetItemQueryIterator<TEntity>(
                query);
            
            return await FeedIteratorToResults(feedIterator);
        }

        protected async Task<List<TEntity>> FeedIteratorToResults<TEntity>(FeedIterator<TEntity> feedIterator)
        {
            List<TEntity> results = new();
            
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<TEntity> response = await feedIterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
        {
            throw new System.NotImplementedException();
        }

        public Task<EntitySearchResult<TEntity>> SearchEntities<TEntity>(IQueryable<TEntity> baseQuery, EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
        {
            throw new System.NotImplementedException();
        }

        public Task<int> RecordCount<TEntity>(EntitySearchQueryModel entitySearch, List<SearchFilterModel> mandatoryFilters) where TEntity : class, IEntityWithId
        {
            throw new System.NotImplementedException();
        }
    }
}