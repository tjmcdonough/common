using Airslip.Common.Repository.Types.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Airslip.Common.Services.MongoDb.Extensions
{
    public static class Extensions
    {
        public static IMongoCollection<TType> CollectionByType<TType>(this IMongoDatabase mongoDatabase)
        {
            return mongoDatabase.GetCollection<TType>(AirslipMongoDbBase.DeriveCollectionName<TType>());
        }

        public static bool CheckCollection(this IMongoDatabase mongoDatabase, string collectionName)
        {
            BsonDocument filter = new("name", collectionName);
            IAsyncCursor<BsonDocument> collectionCursor =
                mongoDatabase.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collectionCursor.Any();
        }
        
        public static void CreateCollectionForEntity<TType>(this IMongoDatabase mongoDatabase) 
            where TType : IEntityWithId
        {
            // Map classes
            AirslipMongoDbBase.MapEntityWithId<TType>();

            string collectionName = AirslipMongoDbBase.DeriveCollectionName<TType>();

            if (!mongoDatabase.CheckCollection(collectionName))
                mongoDatabase.CreateCollection(collectionName);
        }
        
        public static void SetupIndex<TEntity>(this IMongoDatabase mongoDatabase, IndexTypes indexType, string field) 
            where TEntity : class, IEntityWithId 
        {
            CreateIndexOptions indexOptions = new();

            IndexKeysDefinitionBuilder<TEntity> indexBuilder = Builders<TEntity>.IndexKeys;

            IMongoCollection<TEntity> collection = mongoDatabase.CollectionByType<TEntity>();

            IndexKeysDefinition<TEntity>? indexKeysDefinition = indexType switch
            {
                IndexTypes.Ascending => indexBuilder.Ascending(field),
                IndexTypes.Descending => indexBuilder.Descending(field),
                _ => throw new ArgumentOutOfRangeException(nameof(indexType), indexType, null)
            };

            collection.Indexes.CreateOneAsync(
                new CreateIndexModel<TEntity>(
                    indexKeysDefinition,
                    indexOptions));
        }
    }
}