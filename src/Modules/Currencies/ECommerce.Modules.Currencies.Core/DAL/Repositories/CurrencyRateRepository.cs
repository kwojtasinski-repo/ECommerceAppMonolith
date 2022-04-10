using ECommerce.Modules.Currencies.Core.Entities;
using ECommerce.Modules.Currencies.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.DAL.Repositories
{
    internal class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly CurrenciesDbContext _dbContext;

        public CurrencyRateRepository(CurrenciesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CurrencyRate currencyRate)
        {
            await _dbContext.CurrencyRates.AddAsync(currencyRate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CurrencyRate>> GetAllAsync()
        {
            var currencyRates = await _dbContext.CurrencyRates.ToListAsync();
            return currencyRates;
        }

        public async Task<CurrencyRate> GetAsync(Guid id)
        {
            var currencyRate = await _dbContext.CurrencyRates.Include(c => c.Currency).Where(cr => cr.Id == id).SingleOrDefaultAsync();
            return currencyRate;
        }

        public async Task<CurrencyRate> GetCurrencyRateForDateAsync(Guid currencyId, DateOnly date)
        {
            var currencyRate = await _dbContext.CurrencyRates.Include(c => c.Currency)
                .Where(cr => cr.CurrencyId == currencyId && cr.CurrencyDate == date).SingleOrDefaultAsync();
            return currencyRate;
        }

        public Task<IReadOnlyList<CurrencyRate>> GetCurrencyRatesForDateAsync(IEnumerable<string> currencyCodes, DateOnly date)
        {
            var currenciesQueryable = _dbContext.CurrencyRates.AsQueryable();

            var currenciesQueryableFiltered = currenciesQueryable.Where(cr => cr.CurrencyDate == date);

            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in currenciesQueryableFiltered
                                    on currencyCode.ToUpperInvariant() equals currencyRate.Currency.Code.ToUpperInvariant()
                                 select currencyRate).ToList();

            return Task.FromResult<IReadOnlyList<CurrencyRate>>(currencyRates);
        }

        public Task<IReadOnlyList<CurrencyRate>> GetLatestCurrencyRates(IEnumerable<string> currencyCodes)
        {
            var currenciesQueryable = _dbContext.CurrencyRates.AsQueryable();

            var currenciesQueryableFiltered = (from currencyRate in currenciesQueryable
                                               where !_dbContext.CurrencyRates.Any(cr => cr.Currency.Code.ToUpperInvariant() == currencyRate.Currency.Code.ToUpperInvariant()
                                                            && currencyRate.CurrencyDate < cr.CurrencyDate)
                                               select currencyRate);

            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in currenciesQueryableFiltered
                                    on currencyCode.ToUpperInvariant() equals currencyRate.Currency.Code.ToUpperInvariant()
                                 select currencyRate).ToList();

            return Task.FromResult<IReadOnlyList<CurrencyRate>>(currencyRates);
        }

        public async Task UpdateAsync(CurrencyRate currencyRate)
        {
            _dbContext.CurrencyRates.Update(currencyRate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
