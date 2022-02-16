using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    [Collection("integrationBrands")]
    public class BrandsControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
        private const string Path = "items-module/brands";
        private readonly IFlurlClient _client;
        private readonly ItemsDbContext _dbContext;

        public BrandsControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}