using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.MongoDb.Extensions;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{

    public abstract partial class AirslipMongoDbBase
    {
        public async Task<TEntity> AddEntity<TEntity>(TEntity newEntity)
            where TEntity : class, IEntityWithId
        {
            // Find appropriate collection
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            // Add entity to collection
            await collection.InsertOneAsync(newEntity);

            // Return the added entity - likely to be the same object
            return newEntity;
        }

        public async Task<TEntity?> GetEntity<TEntity>(string id)
            where TEntity : class, IEntityWithId
        {
            // Find appropriate collection
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            return await collection
                .Find(entity => entity.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateEntity<TEntity>(TEntity updatedEntity) where TEntity : class, IEntityWithId
        {
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            await collection.ReplaceOneAsync(user => user.Id == updatedEntity.Id, updatedEntity);

            return updatedEntity;
        }

        public async Task<TEntity> UpsertEntity<TEntity>(TEntity newEntity) where TEntity : class, IEntityWithId
        {
            // Find appropriate collection
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            // Add entity to collection
            await collection.ReplaceOneAsync(
                entity => entity.Id == newEntity.Id, newEntity, new ReplaceOptions
                {
                    IsUpsert = true
                });

            // Return the added entity - likely to be the same object
            return newEntity;
        }

        public async Task<TEntity> Update<TEntity>(
            string id, 
            string field, 
            string value)
            where TEntity : class, IEntityWithId
        {
            IMongoCollection<TEntity> collection = Database.CollectionByType<TEntity>();

            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter
                .Eq(bankTransaction => bankTransaction.Id, id);

            UpdateDefinition<TEntity> update = Builders<TEntity>.Update
                .Set(field, value);

            await collection.UpdateOneAsync(filter, update);

            return await collection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }
    }
}