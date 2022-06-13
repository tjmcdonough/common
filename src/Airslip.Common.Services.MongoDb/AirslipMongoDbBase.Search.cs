using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Services.MongoDb.Extensions;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public abstract partial class AirslipMongoDbSearchBase : ISearchContext
    {
        private readonly IUserContext _userContext;
        protected readonly IMongoDatabase Database;

        protected AirslipMongoDbSearchBase(MongoClient mongoClient, IUserContext userContext,  
            IOptions<MongoDbSettings> options)
        {
            _userContext = userContext;
            Database = mongoClient.GetDatabase(options.Value.DatabaseName);
        }
        
        public Task<List<TEntity>> SearchEntities<TEntity>(List<SearchFilterModel> searchFilters)
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

            FilterDefinitionBuilder<TEntity>? filterBuilder = Builders<TEntity>.Filter;
            List<FilterDefinition<TEntity>> filters = new();
            foreach (SearchFilterModel searchFilterModel in searchFilters)
            {
                switch (searchFilterModel.Value)
                {
                    case bool boolValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.ColumnField, boolValue));
                        break;
                    case int intValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.ColumnField, intValue));
                        break;
                    case long lngValue:
                        filters.Add(filterBuilder.Eq(searchFilterModel.ColumnField, lngValue));
                        break;
                    case AirslipUserType airslipUserType:
                        filters.Add(filterBuilder.Eq(searchFilterModel.ColumnField, airslipUserType));
                        break;
                    default:
                        filters.Add(filterBuilder.Eq(searchFilterModel.ColumnField, searchFilterModel.Value
                            .ToString()));
                        break;
                }
            }

            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            return collection
                .Find(filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<TEntity>.Empty)
                .ToListAsync();
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