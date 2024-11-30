using ECommerce.Modules.Users.Core.DAL;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Xunit;

namespace ECommerce.Modules.Users.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestApplicationFactory<Program>>,
        ICollectionFixture<TestUsersDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest
    {
        public const string COLLECTION_TEST_NAME = "integration-users-module-testing";

        protected readonly IFlurlClient client;
        internal readonly UsersDbContext dbContext;

        public BaseTest(TestApplicationFactory<Program> factory, TestUsersDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }
    }
}
