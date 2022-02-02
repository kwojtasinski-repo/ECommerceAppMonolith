using ECommerce.Modules.Currencies.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Repositories
{
    internal class InMemoryCurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly ConcurrentDictionary<Guid, CurrencyRate> _currencyRates = new();

        public Task AddAsync(CurrencyRate currencyRate)
        {
            _currencyRates.TryAdd(currencyRate.Id, currencyRate);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<CurrencyRate>> GetAllAsync()
        {
            return Task.FromResult(_currencyRates.Values.AsEnumerable());
        }

        public Task<CurrencyRate> GetAsync(Guid id)
        {
            _currencyRates.TryGetValue(id, out var currencyRate);
            return Task.FromResult<CurrencyRate>(currencyRate);
        }
    }
}
