using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using WireMock.Server;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    public class CurrenciesBaseTest : BaseIntegrationTest
    {
        /// <summary>
        /// Used WireMock to create plugs
        /// </summary>
        internal readonly WireMockServer WireMockServer;
        internal readonly IFlurlClient Client;
        internal readonly CurrenciesDbContext DbContext;
        internal readonly TestApplicationFactory<Program> Factory;

        public CurrenciesBaseTest(TestApplicationFactory<Program> factory, TestCurrenciesDbContext dbContext)
        {
            WireMockServer = WireMockServer.Start();
            // override config nbpClientOptions
            var options = factory.Services.GetRequiredService<IOptionsMonitor<NbpClientOptions>>();
            options.CurrentValue.BaseUrl = WireMockServer.Urls.Single();
            options.CurrentValue.Timeout = 2;
            var client = factory.CreateClient();
            Client = new FlurlClient(client);
            DbContext = dbContext.DbContext;
            Factory = factory;
        }
    }
}
