using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace Airslip.Common.Services.CosmosDb.Extensions
{
    public static class Extensions
    {
        public static async Task CreateCollection<TEntity>(this Database database, string partitionKeyPath = "id")
        {
            string containerId = AirslipCosmosDbBase.GetContainerId<TEntity>();
            
            await database.CreateContainerIfNotExistsAsync(containerId, $"/{partitionKeyPath}");
        }
        
        public static Container GetContainerForEntity<TEntity>(this Database database)
        {
            string containerId = AirslipCosmosDbBase.GetContainerId<TEntity>();
            return database.GetContainer(containerId);
        }
    }
}