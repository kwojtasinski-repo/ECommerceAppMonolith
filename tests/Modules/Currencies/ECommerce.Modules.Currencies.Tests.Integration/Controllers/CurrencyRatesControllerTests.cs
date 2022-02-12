using ECommerce.Modules.Currencies.Core.Clients;
using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Controllers
{
    public class CurrencyRatesControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestCurrenciesDbContext>
    {
        [Fact]
        public async Task given_valid_id_should_return_latest_currency_rate()
        {
            var currency = new Currency { Id = Guid.NewGuid(), Code = "eur", Description = "euro" };
            await _dbContext.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var content = Common.Extensions.GetSampleCurrencyRateJsonString(date);
            var url = $"/api/exchangerates/rates/a/{currency.Code}/{date.ToString("yyyy-MM-dd")}";
            _wireMockServer.Given(
              Request.Create()
                .WithPath(url)
                .UsingGet()).RespondWith(
              Response.Create()
                .WithBody(content)
                .WithStatusCode(200));

            var response = await _client.Request($"{Path}/{currency.Id}").GetAsync();
            var currencyRate = await response.GetJsonAsync<CurrencyRateDto>();

            currencyRate.CurrencyId.ShouldBe(currency.Id);
            currencyRate.Rate.ShouldBeGreaterThan(decimal.Zero);
        }

        /// <summary>
        /// Do tworzenia zaślepek używam biblioteki WireMock
        /// </summary>
        private readonly WireMockServer _wireMockServer;
        private const string Path = "currencies-module/currency-rates";
        private readonly IFlurlClient _client;
        private readonly CurrenciesDbContext _dbContext;

        public CurrencyRatesControllerTests(TestApplicationFactory<Program> factory, TestCurrenciesDbContext dbContext)
        {
            _wireMockServer = WireMockServer.Start();
            // nadpisuje config nbpClientOptions
            var options = factory.Services.GetRequiredService<IOptions<NbpClientOptions>>();
            options.Value.BaseUrl = _wireMockServer.Urls.Single();
            options.Value.Timeout = 2;
            var client = factory.CreateClient();
            _client = new FlurlClient(client);
            _dbContext = dbContext.DbContext;
        }
    }
}
