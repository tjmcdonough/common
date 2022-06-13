using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.MongoDb.Extensions;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;

namespace Airslip.Common.Services.MongoDb
{
    public abstract partial class AirslipMongoDbBase : IContext
    {
        protected readonly IMongoDatabase Database;

        protected AirslipMongoDbBase(MongoClient mongoClient,  
            IOptions<MongoDbSettings> options)
        {
            Database = mongoClient.GetDatabase(options.Value.DatabaseName);
        }

        public static void MapEntityWithId<TType>() where TType : IEntityWithId
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TType)))
            {
                BsonClassMap.RegisterClassMap<TType>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                });
            }
        }        
        
        public static void MapEntity<TType>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TType)))
            {
                BsonClassMap.RegisterClassMap<TType>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
        
        public IQueryable<TEntity> QueryableOf<TEntity>()
            where TEntity : class
        {
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            return collection.AsQueryable();
        }

        public static string DeriveCollectionName<TType>()
        {
            return $"{typeof(TType).Name}s".ToCamelCase();
        }
    }
}