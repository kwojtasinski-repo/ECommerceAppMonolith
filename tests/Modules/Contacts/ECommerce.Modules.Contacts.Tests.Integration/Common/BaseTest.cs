using ECommerce.Modules.Contacts.Core.DAL;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Xunit;

namespace ECommerce.Modules.Contacts.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestApplicationFactory<Program>>,
        ICollectionFixture<TestContactsDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest
    {
        public const string COLLECTION_TEST_NAME = "integration-contacts-module-testing";

        protected readonly IFlurlClient client;
        internal readonly ContactsDbContext dbContext;

        public BaseTest(TestApplicationFactory<Program> factory, TestContactsDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }
    }
}
