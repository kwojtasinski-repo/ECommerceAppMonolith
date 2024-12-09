using ArangoDBNetStandard;
using ECommerce.Modules.PurchaseProfiler.Core.Database;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ECommerce.Modules.PurchaseProfiler.Tests.Integration.Repositories
{
    public class UserRepositoryTests : IDisposable, IAsyncDisposable, IAsyncLifetime
    {
        [Fact]
        public async Task given_user_entity_should_add_to_database()
        {
            var user = new User
            {
                Email = "email@email.com",
                UserId = Guid.NewGuid()
            };

            var userAdded = await _userRepository.AddAsync(user);

            var userFromDb = await _userRepository.GetByIdAsync(userAdded.KeyValue);
            Assert.NotNull(userAdded);
            Assert.True(userAdded.KeyValue > 0);
            Assert.NotNull(userFromDb);
            Assert.True(userFromDb.KeyValue > 0);
        }

        [Fact]
        public async Task given_valid_id_should_update()
        {
            var user = new User(key: "1")
            {
                Email = "email@email22.com",
                UserId = Guid.NewGuid()
            };

            var userUpdated = await _userRepository.UpdateAsync(user);

            Assert.NotNull(userUpdated);
            var userFromDb = await _userRepository.GetByIdAsync(user.KeyValue);
            Assert.NotNull(userFromDb);
            Assert.True(userFromDb.KeyValue > 0);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var id = 4;

            var deleted = await _userRepository.DeleteAsync(id);

            var userFromDb = await _userRepository.GetByIdAsync(id);
            Assert.True(deleted);
            Assert.Null(userFromDb);
        }

        private readonly IConfiguration _inMemoryConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;

        public UserRepositoryTests()
        {
            _inMemoryConfiguration = new ConfigurationManager()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "arangoDB:url", "http://localhost:8529" },
                    { "arangoDB:database", "ECommerceApp.PurchaseProfiler" },
                    { "arangoDB:username", "root" },
                    { "arangoDB:password", "P4SSW0Rd!1" },
                    { "arangoDB:initializeDatabaseOnStart", "true" }
                }).Build();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(_inMemoryConfiguration);
            serviceCollection.AddArrangoDb();
            serviceCollection.AddSingleton<ILogger<UserRepository>>((_) => NullLogger<UserRepository>.Instance);
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
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
            var client = _serviceProvider.GetRequiredService<IArangoDBClient>();
            var collections = await client.Collection.GetCollectionsAsync();
            var collectionName = "users";
            // TODO: Think how to handle this situation, maybe on startup?
            if (!collections.Result.Any(c => string.Equals(c.Name, collectionName)))
            {
                await client.Collection.PostCollectionAsync(new ArangoDBNetStandard.CollectionApi.Models.PostCollectionBody
                {
                    Name = collectionName,
                    KeyOptions = new ArangoDBNetStandard.CollectionApi.Models.CollectionKeyOptions
                    {
                        Increment = 1,
                        Type = "autoincrement"
                    }
                });
            }
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
