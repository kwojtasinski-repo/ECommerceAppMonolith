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
            var currencyRate = await _dbContext.CurrencyRates.Where(cr => cr.Id == id).SingleOrDefaultAsync();
            return currencyRate;
        }

        public async Task<CurrencyRate> GetCurrencyRateForDateAsync(Guid currencyId, DateOnly date)
        {
            var currencyRate = await _dbContext.CurrencyRates
                .Where(cr => cr.CurrencyId == currencyId && cr.CurrencyDate == date).SingleOrDefaultAsync();
            return currencyRate;
        }
    }
}
