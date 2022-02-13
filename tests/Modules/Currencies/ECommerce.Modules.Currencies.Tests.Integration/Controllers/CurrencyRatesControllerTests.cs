using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Controllers
{
    [Collection("integrationCurrencyRates")]
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

        [Fact]
        public async Task given_valid_id_should_return_latest_currency_rate_from_db()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            var currency = new Currency { Id = Guid.NewGuid(), Code = "chf", Description = "frank", CurrencyRates = new List<CurrencyRate>() };
            var currencyRate = new CurrencyRate { Id = Guid.NewGuid(), Currency = currency, CurrencyId = currency.Id, CurrencyDate = date, Rate = 4.52M };
            currency.CurrencyRates.Add(currencyRate);
            await _dbContext.AddAsync(currency);
            await _dbContext.SaveChangesAsync();

            var response = await _client.Request($"{Path}/{currency.Id}").GetAsync();
            var currencyRateDto = await response.GetJsonAsync<CurrencyRateDto>();

            currencyRateDto.CurrencyId.ShouldBe(currency.Id);
            currencyRateDto.Rate.ShouldBeGreaterThan(decimal.Zero);
            currencyRateDto.Rate.ShouldBe(currencyRate.Rate);
        }

        [Fact]
        public async Task given_invalid_id_should_return_status_not_found()
        {
            var id = Guid.NewGuid();
            var expectedException = new CurrencyNotFoundException(id);

            var response = await _client.Request($"{Path}/{id}").AllowHttpStatus("400").GetAsync();

            response.ShouldNotBeNull();
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldContain(expectedException.CurrencyId.ToString());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_id_and_date_should_return_currency_rate()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            var currency = new Currency { Id = Guid.NewGuid(), Code = "gbp", Description = "funt", CurrencyRates = new List<CurrencyRate>() };
            var currencyRate = new CurrencyRate { Id = Guid.NewGuid(), Currency = currency, CurrencyId = currency.Id, CurrencyDate = date, Rate = 4.52M };
            currency.CurrencyRates.Add(currencyRate);
            await _dbContext.AddAsync(currency);
            await _dbContext.SaveChangesAsync();

            var response = await _client.Request($"{Path}/?currencyId={currency.Id}&date={date}").GetAsync();
            var currencyRateDto = await response.GetJsonAsync<CurrencyRateDto>();

            currencyRateDto.ShouldNotBeNull();
            currencyRateDto.Rate.ShouldBeGreaterThan(decimal.Zero);
            currencyRateDto.Rate.ShouldBe(currencyRate.Rate);
        }

        [Fact]
        public async Task given_invalid_id_and_valid_date_should_return_status_not_found()
        {
            var id = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Now);
            var expectedException = new CurrencyNotFoundException(id);

            var response = await _client.Request($"{Path}/?currencyId={id}&date={date}").AllowHttpStatus("400").GetAsync();

            response.ShouldNotBeNull();
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldContain(expectedException.CurrencyId.ToString());
            exception.Message.ShouldBe(expectedException.Message);
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
