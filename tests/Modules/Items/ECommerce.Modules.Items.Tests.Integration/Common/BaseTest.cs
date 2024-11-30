using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestApplicationFactory<Program>>,
        ICollectionFixture<TestItemsDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest
    {
        public const string COLLECTION_TEST_NAME = "integration-items-module-testing";

        protected readonly IFlurlClient client;
        protected readonly ItemsDbContext dbContext;

        public BaseTest(TestApplicationFactory<Program> factory, TestItemsDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }
    }
}
