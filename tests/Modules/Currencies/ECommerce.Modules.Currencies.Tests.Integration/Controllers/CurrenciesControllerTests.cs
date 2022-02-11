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

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "currencies" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            //_client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
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
        private IFlurlClient _client;
        private CurrenciesDbContext _dbContext;

        public CurrenciesControllerTests(TestApplicationFactory<Program> factory, TestCurrenciesDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
