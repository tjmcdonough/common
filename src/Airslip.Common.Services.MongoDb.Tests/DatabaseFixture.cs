using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.MongoDb.Extensions;
using Airslip.Common.Testing;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Services.MongoDb.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private MongoDbSettings _mongoDbSettings = null!;
        private IContext Context = null!;
        public ISearchContext SearchContext = null!;

        public async Task InitializeAsync()
        {
            IConfiguration config = OptionsMock
                .InitialiseConfiguration("Airslip.Common.Services.MongoDb.Tests")!;
            _mongoDbSettings = UtilityExtensions.GetConfigurationSection<MongoDbSettings>(config);
            _mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = _mongoDbSettings.ConnectionString,
                DatabaseName = $"tests_{CommonFunctions.GetId()}"
            };
            MongoClient client = await Helpers.InitializeMongoClientInstanceAsync(config,
                _ => Task.FromResult(true));
            
            Context = new BaseContext(client, Options.Create(_mongoDbSettings));
            SearchContext = new SearchContext(client, Options.Create(_mongoDbSettings));
            
            string[] names = {"Some Name 1", "Some Name 2", "Some Name 3", "Some Name 4"};
            
            // Prepare some test data
            foreach (string name in names)
            {
                await Context.AddEntity(new MyEntity
                {
                    Id = CommonFunctions.GetId(),
                    Name = name
                });
            }
        }

        public async Task DisposeAsync()
        {
            MongoClient client = new(_mongoDbSettings.ConnectionString);
            await client.DropDatabaseAsync(_mongoDbSettings.DatabaseName);
        }
    }
    
    public class BaseContext : AirslipMongoDbBase
    {
        public BaseContext(MongoClient mongoClient, IOptions<MongoDbSettings> options) 
            : base(mongoClient, options)
        {
            Database.CreateCollectionForEntity<MyEntity>();
        }
    }
    
    public class SearchContext : AirslipMongoDbSearchBase
    {
        public SearchContext(MongoClient mongoClient, IOptions<MongoDbSettings> options) 
            : base(mongoClient, new Mock<IUserContext>().Object, options)
        {
            Database.CreateCollectionForEntity<MyEntity>();
        }
    }

    public class MyEntity : IEntityWithId
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}