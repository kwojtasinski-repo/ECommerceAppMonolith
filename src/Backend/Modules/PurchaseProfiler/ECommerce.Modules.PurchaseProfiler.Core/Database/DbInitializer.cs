using ArangoDBNetStandard.Transport.Http;
using ArangoDBNetStandard;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    internal class DbInitializer(
        DbConfiguration dbConfiguration,
        IOptions<ArangoDatabaseConfig> arangoDatabaseConfig,
        IArangoDBClient arangoDbClient,
        ILogger<DbInitializer> logger)
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (arangoDatabaseConfig.Value.InitializeDatabaseOnStart)
            {
                logger.LogInformation("{methodName}: InitializeDatabaseOnStart is true, initializing database", nameof(StartAsync));
                using var dbClient = new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(arangoDatabaseConfig.Value.Url), arangoDatabaseConfig.Value.RootUserName, arangoDatabaseConfig.Value.RootPassword));
                await CreateDatabaseIfNotExists(dbClient);
            }
            
            logger.LogInformation("{methodName}: initializing collections", nameof(StartAsync));
            await CreateCollectionsIfNotExists();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private async Task CreateDatabaseIfNotExists(ArangoDBClient dbClient)
        {
            var dbs = await dbClient.Database.GetDatabasesAsync();
            foreach (var dbName in dbConfiguration.DatabaseNames)
            {
                if (!dbs.Result.Any(db => string.Equals(dbName, db)))
                {
                    logger.LogInformation("{methodName}: Missing database '{database}', creating...", nameof(CreateDatabaseIfNotExists), dbName);
                    var response = await dbClient.Database.PostDatabaseAsync(new ArangoDBNetStandard.DatabaseApi.Models.PostDatabaseBody
                    {
                        Name = dbName
                    });

                    if (response.Error)
                    {
                        logger.LogError("{methodName}: Creating database '{database}' failed with reason {error}", nameof(CreateDatabaseIfNotExists), dbName, response.Code);
                    }
                    else
                    {
                        logger.LogInformation("{methodName}: Created database '{database}'", nameof(CreateDatabaseIfNotExists), dbName);
                    }
                }
            }
        }

        private async Task CreateCollectionsIfNotExists()
        {
            var collections = await arangoDbClient.Collection.GetCollectionsAsync();
            foreach (var collection in dbConfiguration.Collections)
            {
                if (!collections.Result.Any(c => string.Equals(c.Name, collection.CollectionName)))
                {
                    logger.LogInformation("{methodName}: Missing collection '{collection}', creating...", nameof(CreateCollectionsIfNotExists), collection.CollectionName);
                    var response = await arangoDbClient.Collection.PostCollectionAsync(new ArangoDBNetStandard.CollectionApi.Models.PostCollectionBody
                    {
                        Name = collection.CollectionName,
                        KeyOptions = new ArangoDBNetStandard.CollectionApi.Models.CollectionKeyOptions
                        {
                            Type = collection.KeyGenerationType.ToString().ToLower(),
                            Increment = 1,
                        }
                    });

                    if (response.Error)
                    {
                        logger.LogError("{methodName}: Creating collection '{collection}' failed with reason {error}", nameof(CreateCollectionsIfNotExists), collection.CollectionName, response.Code);
                    }
                    else
                    {
                        logger.LogInformation("{methodName}: Created collection '{collection}'", nameof(CreateCollectionsIfNotExists), collection.CollectionName);
                    }
                }
            }
        }
    }
}
