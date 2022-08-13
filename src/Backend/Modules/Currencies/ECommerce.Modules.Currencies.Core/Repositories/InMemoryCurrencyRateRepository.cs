﻿using ECommerce.Modules.Currencies.Core.Entities;
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

        public Task<IReadOnlyList<CurrencyRate>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<CurrencyRate>>(_currencyRates.Values.ToList());
        }

        public Task<CurrencyRate> GetAsync(Guid id)
        {
            _currencyRates.TryGetValue(id, out var currencyRate);
            return Task.FromResult<CurrencyRate>(currencyRate);
        }

        public Task<CurrencyRate> GetCurrencyRateForDateAsync(Guid currencyId, DateOnly date)
        {
            var currencyRate = _currencyRates.Values.Where(cr => cr.CurrencyId == currencyId && cr.CurrencyDate == date).SingleOrDefault();
            return Task.FromResult<CurrencyRate>(currencyRate);
        }

        public Task<IReadOnlyList<CurrencyRate>> GetCurrencyRatesForDateAsync(IEnumerable<string> currencyCodes, DateOnly date)
        {
            var currencyRates = (from currencyCode in currencyCodes
                                 join currencyRate in _currencyRates.Values
                                    on currencyCode equals currencyRate.Currency.Code
                                 where currencyRate.CurrencyDate == date
                                 select currencyRate).ToList();

            return Task.FromResult<IReadOnlyList<CurrencyRate>>(currencyRates);
        }

        public Task<IReadOnlyList<CurrencyRate>> GetLatestCurrencyRates(IEnumerable<string> currencyCodes)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CurrencyRate rate)
        {
            return Task.CompletedTask;
        }
    }
}
