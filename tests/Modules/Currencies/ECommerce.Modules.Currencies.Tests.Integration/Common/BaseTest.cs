using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestCurrenciesAppFactory>,
        ICollectionFixture<TestCurrenciesDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest, IAsyncLifetime
    {
        public const string COLLECTION_TEST_NAME = "integration-currencies-module-testing";

        protected readonly IFlurlClient client;
        internal readonly CurrenciesDbContext dbContext;
        internal IEnumerable<Currency> currencies = [];

        public BaseTest(TestCurrenciesAppFactory factory, TestCurrenciesDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }

        public async Task InitializeAsync()
        {
            currencies = await dbContext.Currencies.ToListAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
