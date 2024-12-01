using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WireMock.Server;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Controllers
{
    public class CurrencyRatesControllerTests : CurrenciesBaseTest
    {
        [Fact]
        public async Task given_valid_id_should_return_latest_currency_rate_from_db()
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var currency = new Currency { Id = Guid.NewGuid(), Code = "wow", Description = "frank", CurrencyRates = new List<CurrencyRate>() };
            var currencyRate = new CurrencyRate { Id = Guid.NewGuid(), Currency = currency, CurrencyId = currency.Id, CurrencyDate = date, Rate = 4.52M };
            currency.CurrencyRates.Add(currencyRate);
            await dbContext.AddAsync(currency);
            await dbContext.SaveChangesAsync();

            var response = await client.Request($"{Path}/{currency.Id}").GetAsync();
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

            var response = await client.Request($"{Path}/{id}").AllowHttpStatus("400").GetAsync();

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
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var currency = new Currency { Id = Guid.NewGuid(), Code = "a2a", Description = "funt", CurrencyRates = new List<CurrencyRate>() };
            var currencyRate = new CurrencyRate { Id = Guid.NewGuid(), Currency = currency, CurrencyId = currency.Id, CurrencyDate = date, Rate = 4.52M };
            currency.CurrencyRates.Add(currencyRate);
            await dbContext.AddAsync(currency);
            await dbContext.SaveChangesAsync();

            var response = await client.Request($"{Path}/?currencyId={currency.Id}&date={date}").GetAsync();
            var currencyRateDto = await response.GetJsonAsync<CurrencyRateDto>();

            currencyRateDto.ShouldNotBeNull();
            currencyRateDto.Rate.ShouldBeGreaterThan(decimal.Zero);
            currencyRateDto.Rate.ShouldBe(currencyRate.Rate);
        }

        [Fact]
        public async Task given_invalid_id_and_valid_date_should_return_status_not_found()
        {
            var id = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var expectedException = new CurrencyNotFoundException(id);

            var response = await client.Request($"{Path}/?currencyId={id}&date={date}").AllowHttpStatus("400").GetAsync();

            response.ShouldNotBeNull();
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldContain(expectedException.CurrencyId.ToString());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task should_return_currencies()
        {
            var response = await client.Request($"{Path}/latest").GetAsync();
            var currencyRateDtos = await response.GetJsonAsync<IEnumerable<CurrencyRateDto>>();

            currencyRateDtos.ShouldNotBeNull();
            currencyRateDtos.ShouldNotBeEmpty();
        }

        private readonly WireMockServer _wireMockServer;
        private const string Path = "currencies-module/currency-rates";

        public CurrencyRatesControllerTests(TestCurrenciesAppFactory factory, TestCurrenciesDbContext dbContext) : base(factory, dbContext)
        {
            _wireMockServer = WireMockServer;
        }
    }
}
