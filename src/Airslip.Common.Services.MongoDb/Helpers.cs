using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.MongoDb
{
    public static class Helpers
    {
        public static async Task<MongoClient> InitializeMongoClientInstanceAsync(IConfiguration configuration,
            Func<IMongoDatabase, Task> initialiseDatabase)
        {
            MongoDbSettings settings = new();
            configuration.GetSection(nameof(MongoDbSettings))
                .Bind(settings);
            
            MongoUrl url = new(settings.ConnectionString);
            MongoClientSettings? mongoClientSettings = MongoClientSettings.FromUrl(url);

            TelemetryClient? client = Monitoring.Helpers.GetTelemetryClient();
            if (client != null)
            {
                mongoClientSettings.ClusterConfigurator = clusterConfigurator =>
                {
                    clusterConfigurator.Subscribe<CommandSucceededEvent>(e =>
                    {
                        client.TrackDependency("MongoDb", e.CommandName, e.Reply.ToString(), DateTime.Now.Subtract(e.Duration), e.Duration, true);
                    });
                    clusterConfigurator.Subscribe<CommandFailedEvent>(e =>
                    {
                        client.TrackDependency("MongoDb", $"{e.CommandName} - {e.ToString()}", e.Failure.ToString(), DateTime.Now.Subtract(e.Duration), e.Duration, false);
                    });
                };   
            }
            
            MongoClient mongoClient = new(mongoClientSettings);
            IMongoDatabase database = mongoClient.GetDatabase(settings.DatabaseName);

            // General initialisation
            ConventionRegistry.Register(
                "CustomConventionPack",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                t => true);

            // Enum as string
            BsonSerializer.RegisterSerializer(new EnumSerializer<AirslipUserType>(BsonType.String));

            await initialiseDatabase(database);

            return mongoClient;
        }
    }
}