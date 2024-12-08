using ArangoDBNetStandard;
using ArangoDBNetStandard.Transport.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    internal static class Extensions
    {
        public static IServiceCollection AddArrangoDb(this IServiceCollection services)
        {
            var options = services.GetOptions<ArangoDatabaseConfig>("arangoDB");
            services.Configure<ArangoDatabaseConfig>(config =>
            {
                config.Url = options.Url;
                config.Database = options.Database;
                config.UserName = options.UserName;
                config.Password = options.Password;
                config.InitializeDatabaseOnStart = options.InitializeDatabaseOnStart;
            });
            services.AddSingleton((Func<IServiceProvider, IArangoDBClient>)(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ArangoDatabaseConfig>>();
                config.Value.Validate();
                var client = new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(config.Value.Url), config.Value.Database, config.Value.UserName, config.Value.Password));
                if (config.Value.InitializeDatabaseOnStart)
                {
                    CreateDatabaseIfNotExists(config);
                }

                return client;
            }));
            return services;
        }

        private static void CreateDatabaseIfNotExists(IOptions<ArangoDatabaseConfig> config)
        {
            using var dbClient = new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(config.Value.Url), config.Value.UserName, config.Value.Password));
            var dbs = dbClient.Database.GetDatabasesAsync().GetAwaiter().GetResult();
            if (!dbs.Result.Any(db => db == config.Value.Database))
            {
                dbClient.Database.PostDatabaseAsync(new ArangoDBNetStandard.DatabaseApi.Models.PostDatabaseBody
                {
                    Name = config.Value.Database
                }).GetAwaiter().GetResult();
            }
        }

        private static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        private static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
