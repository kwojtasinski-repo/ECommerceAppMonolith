using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Core.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Repositories
{
    internal class InMemoryAddressRepository : IAddressRepository
    {
        private readonly ConcurrentDictionary<Guid, Address> _addresses = new();

        public Task AddAsync(Address currencyRate)
        {
            _addresses.TryAdd(currencyRate.Id, currencyRate);
            return Task.CompletedTask;
        }

        public Task<Address> GetAsync(Guid id)
        {
            _addresses.TryGetValue(id, out var currencyRate);
            return Task.FromResult<Address>(currencyRate);
        }

        public Task UpdateAsync(Address address)
        {
            return Task.CompletedTask;
        }
    }
}
