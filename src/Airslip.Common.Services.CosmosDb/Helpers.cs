using Airslip.Common.Services.CosmosDb.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb
{
    internal static class Helpers
    {
        internal static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfiguration configuration,
            Func<Database, Task> initialiseCollections, 
            ConnectionMode connectionMode = ConnectionMode.Direct,
            ConsistencyLevel consistencyLevel = ConsistencyLevel.Session)
        {
            CosmosDbSettings settings = new();
            configuration.GetSection(nameof(CosmosDbSettings)).Bind(settings);

            CosmosClientOptions options = new()
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
                ConnectionMode = connectionMode,
                ConsistencyLevel = consistencyLevel
            };
            
            CosmosClient client = new(settings.ConnectionString, options);
            
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(settings.DatabaseName);
            
            await initialiseCollections(database.Database);

            return client;
        }
    }
}