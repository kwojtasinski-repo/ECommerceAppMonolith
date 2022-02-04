using ECommerce.Modules.Currencies.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Repositories
{
    internal class InMemoryCurrencyRepository : ICurrencyRepository
    {
        private readonly ConcurrentDictionary<Guid, Currency> _currencies = new();

        public Task AddAsync(Currency currency)
        {
            _currencies.TryAdd(currency.Id, currency);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Currency currency)
        {
            _currencies.TryRemove(new KeyValuePair<Guid, Currency>(currency.Id, currency));
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Currency>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Currency>> (_currencies.Values.ToList());
        }

        public Task<Currency> GetAsync(Guid id)
        {
            _currencies.TryGetValue(id, out var currency);
            return Task.FromResult<Currency>(currency);
        }

        public Task<Currency> GetDetailsAsync(Guid id)
        {
            _currencies.TryGetValue(id, out var currency);
            return Task.FromResult<Currency>(currency);
        }

        public Task UpdateAsync(Currency currency)
        {
            return Task.CompletedTask;
        }
    }
}
