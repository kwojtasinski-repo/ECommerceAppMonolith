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
                config.Url = config.Url;
                config.Database = config.Database;
                config.UserName = config.UserName;
                config.Password = config.Password;
            });
            services.AddSingleton<IArangoDBClient>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ArangoDatabaseConfig>>();
                config.Value.Validate();
                return new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(config.Value.Url), config.Value.Database, config.Value.UserName, config.Value.Password));
            });
            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
