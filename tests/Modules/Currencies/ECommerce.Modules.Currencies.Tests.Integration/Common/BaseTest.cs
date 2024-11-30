using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    [CollectionDefinition(BaseTest.COLLECTION_TEST_NAME)]
    public class ClassFixture : ICollectionFixture<TestApplicationFactory<Program>>,
        ICollectionFixture<TestCurrenciesDbContext>
    {
    }

    [Collection(COLLECTION_TEST_NAME)]
    public class BaseTest : BaseIntegrationTest, IAsyncLifetime
    {
        public const string COLLECTION_TEST_NAME = "integration-currencies-module-testing";

        protected readonly IFlurlClient client;
        internal readonly CurrenciesDbContext dbContext;
        internal IEnumerable<Currency> currencies = [];

        public BaseTest(TestApplicationFactory<Program> factory, TestCurrenciesDbContext testDbContext)
        {
            client = new FlurlClient(factory.CreateClient());
            dbContext = testDbContext.DbContext;
        }

        public async Task InitializeAsync()
        {
            await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        internal Currency GetEurCurrency()
        {
            return currencies.FirstOrDefault(c => c.Code == "EUR")
                ?? throw new InvalidOperationException("Cannot find EUR currency check IAsyncLifetime methods");
        }

        internal Currency GetUsdCurrency()
        {
            return currencies.FirstOrDefault(c => c.Code == "USD")
                ?? throw new InvalidOperationException("Cannot find USD currency check IAsyncLifetime methods");
        }

        internal Currency GetChfCurrency()
        {
            return currencies.FirstOrDefault(c => c.Code == "CHF")
                ?? throw new InvalidOperationException("Cannot find CHF currency check IAsyncLifetime methods");
        }

        internal Currency GetGbpCurrency()
        {
            return currencies.FirstOrDefault(c => c.Code == "GBP")
                ?? throw new InvalidOperationException("Cannot find GBP currency check IAsyncLifetime methods");
        }

        protected async Task AddSampleData()
        {
            var currencyPln = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "PLN")
                ?? new Currency { Id = Guid.NewGuid(), Code = "PLN", Description = "Polski złoty" };
            var currencyEur = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "EUR")
                ?? new Currency { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro" };
            var currencyUsd = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "USD")
                ?? new Currency { Id = Guid.NewGuid(), Code = "USD", Description = "Dolar amerykański" };
            var currencyChf = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "CHF")
                ?? new Currency { Id = Guid.NewGuid(), Code = "CHF", Description = "Frank szwajcarski" };
            var currencyGbp = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "GBP")
                ?? new Currency { Id = Guid.NewGuid(), Code = "GBP", Description = "Funt brytyjski" };

            var currencyDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyPln.Id && cr.CurrencyDate == currencyDate))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyPln, CurrencyDate = currencyDate, CurrencyId = currencyPln.Id, Rate = decimal.One });
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyEur.Id && cr.CurrencyDate == currencyDate))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyEur, CurrencyDate = currencyDate, CurrencyId = currencyEur.Id, Rate = 4.2512M });
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyUsd.Id && cr.CurrencyDate == currencyDate))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyUsd, CurrencyDate = currencyDate, CurrencyId = currencyUsd.Id, Rate = 2.5123M });
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyChf.Id && cr.CurrencyDate == currencyDate))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyChf, CurrencyDate = currencyDate, CurrencyId = currencyChf.Id, Rate = 3.5632M });
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyGbp.Id && cr.CurrencyDate == currencyDate))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyGbp, CurrencyDate = currencyDate, CurrencyId = currencyGbp.Id, Rate = 5.2645M });
            }

            await dbContext.SaveChangesAsync();
            currencies = [currencyPln, currencyEur, currencyUsd, currencyChf, currencyGbp];
        }
    }
}
