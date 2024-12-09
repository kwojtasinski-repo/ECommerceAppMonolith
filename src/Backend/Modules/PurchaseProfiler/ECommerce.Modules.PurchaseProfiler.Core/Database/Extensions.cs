using ArangoDBNetStandard;
using ArangoDBNetStandard.Transport.Http;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
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
                config.RootUserName = options.RootUserName;
                config.RootPassword = options.RootPassword;
            });
            services.AddSingleton((sp) => new DbConfiguration
            {
                DatabaseNames = [ sp.GetRequiredService<IOptions<ArangoDatabaseConfig>>().Value.Database ],
                Collections = typeof(Extensions).Assembly.GetTypes()
                                .AsParallel()
                                .Where(c => c.IsClass && !c.IsAbstract)
                                .Where(c => c.GetInterfaces()
                                             .Any(i => i.IsGenericType &&
                                                       i.GetGenericTypeDefinition() == typeof(IDocumentEntity<>)))
                                .Select(type =>
                                {
                                    var instance = Activator.CreateInstance(type) as dynamic;
                                    var keyGenTypeAttribute = GetKeyGenerationTypeAttribute(type);
                                    var keyGenerationType = keyGenTypeAttribute?.KeyGenerationType;

                                    return new CollectionInfo
                                    {
                                        CollectionName = instance?.CollectionName ?? string.Empty,
                                        KeyGenerationType = keyGenerationType ?? KeyGenerationType.Autoincrement
                                    };
                                })
                                .Where(collectionInfo => collectionInfo is not null)
                                .OfType<CollectionInfo>()
                                .ToList()
            });
            services.AddSingleton((Func<IServiceProvider, IArangoDBClient>)(sp =>
            {
                var config = sp.GetRequiredService<IOptions<ArangoDatabaseConfig>>();
                config.Value.Validate();
                return new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(config.Value.Url), config.Value.Database, config.Value.UserName, config.Value.Password));
            }));
            services.AddHostedService<DbInitializer>();
            return services;
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

        private static KeyGenerationTypeAttribute GetKeyGenerationTypeAttribute(Type type)
        {
            // Check for the attribute on the current type
            var attribute = type.GetCustomAttributes(typeof(KeyGenerationTypeAttribute), false)
                                .FirstOrDefault() as KeyGenerationTypeAttribute;
            if (attribute != null)
            {
                return attribute; // Found on the current type
            }

            // Traverse the inheritance hierarchy to find the attribute on a base type
            var baseType = type.BaseType;
            while (baseType != null)
            {
                attribute = baseType.GetCustomAttributes(typeof(KeyGenerationTypeAttribute), false)
                                    .FirstOrDefault() as KeyGenerationTypeAttribute;
                if (attribute != null)
                {
                    return attribute; // Found on a base type
                }
                baseType = baseType.BaseType; // Move up the hierarchy
            }

            return null; // No attribute found
        }
    }
}
