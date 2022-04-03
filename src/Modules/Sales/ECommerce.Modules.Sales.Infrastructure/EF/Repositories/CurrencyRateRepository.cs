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
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.CurrencyCode == currencyCode && cr.Created == createdDate).AsNoTracking().SingleOrDefaultAsync();
            return currencyRate != null;
        }

        public async Task<CurrencyRate> GetCurrencyRate(string currencyCode, DateOnly createdDate)
        {
            var currencyRate = await _salesDbContext.CurrencyRates.Where(cr => cr.CurrencyCode == currencyCode && cr.Created == createdDate).SingleOrDefaultAsync();
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

            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in currenciesQueryable
                                    on currencyCode equals currencyRate.CurrencyCode
                                 where currencyRate.Created == date
                                 select currencyRate).ToList();

            return Task.FromResult<IEnumerable<CurrencyRate>>(currencyRates);
        }
    }
}
