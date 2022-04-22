using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public CurrencyRateRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(CurrencyRate currencyRate)
        {
            await _salesDbContext.CurrencyRates.AddAsync(currencyRate);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.Id == id).AsNoTracking().SingleOrDefaultAsync();
            return currencyRate != null;
        }

        public async Task<bool> ExistsAsync(string currencyCode, DateOnly createdDate)
        {
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.CurrencyCode.ToUpperInvariant() == currencyCode.ToUpperInvariant() && cr.Created == createdDate).AsNoTracking().SingleOrDefaultAsync();
            return currencyRate != null;
        }

        public async Task<CurrencyRate> GetCurrencyRate(string currencyCode, DateOnly createdDate)
        {
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.CurrencyCode.ToUpperInvariant() == currencyCode.ToUpperInvariant() && cr.Created == createdDate).SingleOrDefaultAsync();
            return currencyRate;
        }

        public async Task UpdateAsync(CurrencyRate currencyRate)
        {
            _salesDbContext.CurrencyRates.Update(currencyRate);
            await _salesDbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<CurrencyRate>> GetCurrencyRatesForDate(IEnumerable<string> currencyCodes, DateOnly date)
        {
            var currenciesQueryable = _salesDbContext.CurrencyRates.AsQueryable();

            var currenciesQueryableFiltered = currenciesQueryable.Where(cr => cr.Created == date);

            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in currenciesQueryableFiltered
                                    on currencyCode equals currencyRate.CurrencyCode
                                 select currencyRate).ToList();

            return Task.FromResult<IEnumerable<CurrencyRate>>(currencyRates);
        }

        public async Task<CurrencyRate> GetAsync(Guid currencyRateId)
        {
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.Id == currencyRateId).SingleOrDefaultAsync();
            return currencyRate;
        }

        public Task<IEnumerable<CurrencyRate>> GetLatestCurrencyRates(IEnumerable<string> currencyCodes)
        {
            var currenciesQueryable = _salesDbContext.CurrencyRates.AsQueryable();

            // Where !_salesDbContext.CurrencyRates.Any(...) =
            // NOT (EXISTS (SELECT 1 FROM sales."CurrencyRates" AS c0
            // WHERE(c0."CurrencyCode" = c."CurrencyCode") AND(c."RateDate" < c0."RateDate")))
            var currenciesQueryableFiltered = (from currencyRate in currenciesQueryable
                                               where !_salesDbContext.CurrencyRates.Any(cr => cr.CurrencyCode == currencyRate.CurrencyCode
                                                            && currencyRate.RateDate < cr.RateDate)
                                               select currencyRate);

            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in currenciesQueryableFiltered
                                    on currencyCode equals currencyRate.CurrencyCode
                                 select currencyRate).ToList();

            return Task.FromResult<IEnumerable<CurrencyRate>>(currencyRates);
        }

        public async Task<CurrencyRate> GetLatestCurrencyRate(string currencyCode)
        {
            var currenciesQueryable = _salesDbContext.CurrencyRates.AsQueryable().Where(cr => cr.CurrencyCode == currencyCode);

            // Where !_salesDbContext.CurrencyRates.Any(...) =
            // NOT (EXISTS (SELECT 1 FROM sales."CurrencyRates" AS c0
            // WHERE(c0."CurrencyCode" = c."CurrencyCode") AND(c."RateDate" < c0."RateDate")))
            var currenciesQueryableFiltered = (from currencyRate in currenciesQueryable
                                               where !_salesDbContext.CurrencyRates.Any(cr => cr.CurrencyCode == currencyRate.CurrencyCode
                                                            && currencyRate.RateDate < cr.RateDate)
                                               select currencyRate);

            var rate = await currenciesQueryableFiltered.SingleOrDefaultAsync();

            return rate;
        }
    }
}
