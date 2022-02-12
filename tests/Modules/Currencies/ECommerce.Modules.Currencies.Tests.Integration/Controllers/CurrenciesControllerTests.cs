using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Controllers
{
    [Collection("integration")]
    public class CurrenciesControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestCurrenciesDbContext>
    {
        [Fact]
        public async Task should_return_currencies()
        {
            var currencies = GetSampleData();
            await AddSampleData();

            var response = (await _client.Request($"{Path}").GetAsync());
            var currenciesFromDb = await response.GetJsonAsync<IEnumerable<CurrencyDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            currenciesFromDb.ShouldNotBeNull();
            currenciesFromDb.Count().ShouldBe(currencies.Count);
        }

        [Fact]
        public async Task given_unauthorized_user_should_return_401()
        {
            var currencyDto = new CurrencyDto { Code = "USD", Description = "Dolar" };

            var response = await Record.ExceptionAsync(() => _client.Request($"{Path}").PostJsonAsync(currencyDto));

            ((FlurlHttpException) response).StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task given_valid_currency_should_add_and_return_201()
        {
            var currencyDto = new CurrencyDto { Code = "USD", Description = "Dolar" };
            var userId = Guid.NewGuid();
            Authenticate(userId);

            var response = (await _client.Request($"{Path}").PostJsonAsync(currencyDto));

            response.StatusCode.ShouldBe((int) HttpStatusCode.Created);
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task given_valid_currency_should_add_to_db()
        {
            var currencyDto = new CurrencyDto { Code = "CHF", Description = "Frank" };
            var userId = Guid.NewGuid();
            Authenticate(userId);

            var response = (await _client.Request($"{Path}").PostJsonAsync(currencyDto));
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var currency = _dbContext.Currencies.Where(c => c.Id == id).SingleOrDefault();

            currency.ShouldNotBeNull();
            currency.Code.ShouldBe(currencyDto.Code);
            currency.Description.ShouldBe(currencyDto.Description);
        }

        [Fact]
        public async Task given_valid_currency_should_update()
        {
            var currency = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "ABC", Description = "Abcedfgh" };
            await _dbContext.Currencies.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
            var dto = new CurrencyDto { Id = currency.Id, Code = "PLN", Description = "Polski zloty" };
            var userId = Guid.NewGuid();
            Authenticate(userId);

            await _client.Request($"{Path}/{dto.Id}").PutJsonAsync(dto);
            var dtoUpdated = await _client.Request($"{Path}/{dto.Id}").GetJsonAsync<CurrencyDto>();

            dtoUpdated.ShouldNotBeNull();
            dtoUpdated.Code.ShouldBe(dto.Code);
            dtoUpdated.Description.ShouldBe(dto.Description);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var currency = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "BUG", Description = "Abcedfgh" };
            await _dbContext.Currencies.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
            var userId = Guid.NewGuid();
            Authenticate(userId);

            await _client.Request($"{Path}/{currency.Id}").DeleteAsync();
            var response = await Record.ExceptionAsync(() => _client.Request($"{Path}/{currency.Id}").GetJsonAsync<CurrencyDto>());

            ((FlurlHttpException) response).StatusCode.ShouldBe((int) HttpStatusCode.NotFound);
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "currencies" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private async Task AddSampleData()
        {
            var currencies = GetSampleData();
            var currency1 = currencies[0];
            var currency2 = currencies[1];
            await _dbContext.Currencies.AddAsync(currency1);
            await _dbContext.Currencies.AddAsync(currency2);
            await _dbContext.SaveChangesAsync();
        }

        private List<Core.Entities.Currency> GetSampleData()
        {
            var currency1 = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "PLN", Description = "Złoty" };
            var currency2 = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro" };
            return new List<Core.Entities.Currency> { currency1, currency2 };
        }

        private const string Path = "currencies-module/currencies";
        private readonly IFlurlClient _client;
        private readonly CurrenciesDbContext _dbContext;

        public CurrenciesControllerTests(TestApplicationFactory<Program> factory, TestCurrenciesDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
