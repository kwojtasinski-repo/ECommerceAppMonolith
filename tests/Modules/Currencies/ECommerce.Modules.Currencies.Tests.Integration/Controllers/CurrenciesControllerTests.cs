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
    public class CurrenciesControllerTests : CurrenciesBaseTest
    {
        [Fact]
        public async Task should_return_currencies()
        {
            var response = (await client.Request($"{Path}").GetAsync());
            var currenciesFromDb = await response.GetJsonAsync<IEnumerable<CurrencyDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            currenciesFromDb.ShouldNotBeNull();
            currenciesFromDb.Count().ShouldBe(currencies.Count());
        }

        [Fact]
        public async Task given_unauthorized_user_should_return_401()
        {
            var currencyDto = new CurrencyDto { Code = "USD", Description = "Dolar" };

            var response = await Record.ExceptionAsync(() => client.Request($"{Path}").PostJsonAsync(currencyDto));

            ((FlurlHttpException) response).StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task given_valid_currency_should_add_and_return_201()
        {
            var currencyDto = new CurrencyDto { Code = "CZK", Description = "Czeska korona" };
            var userId = Guid.NewGuid();
            Authenticate(userId, client);

            var response = (await client.Request($"{Path}").PostJsonAsync(currencyDto));

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
            var currencyDto = new CurrencyDto { Code = "OIU", Description = "Funt brytyjski" };
            var userId = Guid.NewGuid();
            Authenticate(userId, client);

            var response = (await client.Request($"{Path}").PostJsonAsync(currencyDto));
            var id = response.GetIdFromHeaders<Guid>(Path);
            var currency = dbContext.Currencies.Where(c => c.Id == id).SingleOrDefault();

            currency.ShouldNotBeNull();
            currency.Code.ShouldBe(currencyDto.Code);
            currency.Description.ShouldBe(currencyDto.Description);
        }

        [Fact]
        public async Task given_valid_currency_should_update()
        {
            var currency = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "ABC", Description = "Abcedfgh" };
            await dbContext.Currencies.AddAsync(currency);
            await dbContext.SaveChangesAsync();
            var dto = new CurrencyDto { Id = currency.Id, Code = "CAO", Description = "Frank szwajcarski" };
            var userId = Guid.NewGuid();
            Authenticate(userId, client);

            await client.Request($"{Path}/{dto.Id}").PutJsonAsync(dto);
            var dtoUpdated = await client.Request($"{Path}/{dto.Id}").GetJsonAsync<CurrencyDto>();

            dtoUpdated.ShouldNotBeNull();
            dtoUpdated.Code.ShouldBe(dto.Code);
            dtoUpdated.Description.ShouldBe(dto.Description);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var currency = new Core.Entities.Currency { Id = Guid.NewGuid(), Code = "BUG", Description = "Abcedfgh" };
            await dbContext.Currencies.AddAsync(currency);
            await dbContext.SaveChangesAsync();
            var userId = Guid.NewGuid();
            Authenticate(userId, client);

            await client.Request($"{Path}/{currency.Id}").DeleteAsync();
            var response = await Record.ExceptionAsync(() => client.Request($"{Path}/{currency.Id}").GetJsonAsync<CurrencyDto>());

            ((FlurlHttpException) response).StatusCode.ShouldBe((int) HttpStatusCode.NotFound);
        }

        private const string Path = "currencies-module/currencies";

        public CurrenciesControllerTests(TestApplicationFactory<Program> factory, TestCurrenciesDbContext dbContext)
            : base(factory, dbContext)
        {
        }
    }
}
