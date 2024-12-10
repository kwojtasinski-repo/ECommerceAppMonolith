using ArangoDBNetStandard;
using ArangoDBNetStandard.Transport.Http;
using ECommerce.Modules.PurchaseProfiler.Core.Database;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ECommerce.Modules.PurchaseProfiler.Tests.Integration.Repositories
{
    public class UserRepositoryTests : IDisposable, IAsyncDisposable, IAsyncLifetime
    {
        [Fact]
        public async Task given_user_entity_should_add_to_database()
        {
            var user = CreateUser("email@email.com");

            var userAdded = await _userRepository.AddAsync(user);

            var userFromDb = await _userRepository.GetByKeyAsync(userAdded.Key!);
            Assert.NotNull(userAdded);
            Assert.True(userAdded.KeyValue > 0);
            Assert.NotNull(userFromDb);
            Assert.True(userFromDb.KeyValue > 0);
        }

        [Fact]
        public async Task given_valid_id_should_update()
        {
            var user = await AddUser();
            user.Email = "email@email22.com";

            var userUpdated = await _userRepository.UpdateAsync(user);

            Assert.NotNull(userUpdated);
            var userFromDb = await _userRepository.GetByKeyAsync(user.Key!);
            Assert.NotNull(userFromDb);
            Assert.True(userFromDb.KeyValue > 0);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var user = await AddUser();

            var deleted = await _userRepository.DeleteAsync(user.Key!);

            var userFromDb = await _userRepository.GetByKeyAsync(user.Key!);
            Assert.True(deleted);
            Assert.Null(userFromDb);
        }

        private readonly IConfiguration _inMemoryConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;
        private const string databaseName = "ECommerceApp.PurchaseProfiler.Test";

        public UserRepositoryTests()
        {
            _inMemoryConfiguration = new ConfigurationManager()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "arangoDB:url", "http://localhost:8529" },
                    { "arangoDB:database", databaseName },
                    { "arangoDB:userName", "root" },
                    { "arangoDB:password", "P4SSW0Rd!1" },
                    { "arangoDB:rootUsername", "root" },
                    { "arangoDB:rootPassword", "P4SSW0Rd!1" },
                    { "arangoDB:initializeDatabaseOnStart", "true" }
                }).Build();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(_inMemoryConfiguration);
            serviceCollection.AddArrangoDb();
            serviceCollection.AddRepositories();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
        }

        public void Dispose()
        {
            if (_serviceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_serviceProvider is IAsyncDisposable asyncDisposableServiceProvider)
            {
                await asyncDisposableServiceProvider.DisposeAsync();
            }
        }

        public async Task InitializeAsync()
        {
            var hostedServices = _serviceProvider.GetServices<IHostedService>();
            var dbInitializer = hostedServices.FirstOrDefault(c => typeof(DbInitializer).IsAssignableFrom(c.GetType()))
                ?? throw new InvalidOperationException("Unable to load integration tests, check if DbInitializer was registered");
            await dbInitializer.StartAsync(CancellationToken.None);
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            var config = _serviceProvider.GetRequiredService<IOptions<ArangoDatabaseConfig>>();
            using var client = new ArangoDBClient(HttpApiTransport.UsingBasicAuth(new Uri(config.Value.Url), config.Value.RootUserName, config.Value.RootPassword));
            await client.Database.DeleteDatabaseAsync(databaseName);
        }

        private async Task<User> AddUser(string email = "email@email.com", Guid? userId = null)
        {
            return await _userRepository.AddAsync(CreateUser(email, userId));
        }

        private User CreateUser(string email = "email@email.com", Guid? userId = null)
        {
            return new User
            {
                Email = "email@email22.com",
                UserId = userId ?? Guid.NewGuid()
            };
        }
    }
}
