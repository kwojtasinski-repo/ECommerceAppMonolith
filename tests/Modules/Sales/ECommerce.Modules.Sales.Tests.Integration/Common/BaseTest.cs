using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestApplicationFactory<Program>>,
        ICollectionFixture<TestSalesDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest
    {
        public const string COLLECTION_TEST_NAME = "integration-sales-module-testing";

        protected readonly IFlurlClient client;
        protected readonly SalesDbContext dbContext;

        public BaseTest(TestApplicationFactory<Program> factory, TestSalesDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }
    }
}
