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
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrenciesDbContext _dbContext;

        public CurrencyRepository(CurrenciesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Currency currency)
        {
            await _dbContext.Currencies.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Currency currency)
        {
            _dbContext.Currencies.Remove(currency);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Currency>> GetAllAsync()
        {
            var currencies = await _dbContext.Currencies.ToListAsync();
            return currencies;
        }

        public Task<Currency> GetAsync(Guid id)
        {
            var currency = _dbContext.Currencies.SingleOrDefaultAsync(c => c.Id == id);
            return currency;
        }
        
        public Task<Currency> GetDetailsAsync(Guid id)
        {
            var currency = _dbContext.Currencies.Include(cr => cr.CurrencyRates).SingleOrDefaultAsync(c => c.Id == id);
            return currency;
        }

        public async Task UpdateAsync(Currency currency)
        {
            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
        }
    }
}
