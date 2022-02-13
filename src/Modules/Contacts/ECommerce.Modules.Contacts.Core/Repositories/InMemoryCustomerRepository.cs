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
    internal class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly ConcurrentDictionary<Guid, Customer> _customers = new();

        public Task AddAsync(Customer customer)
        {
            _customers.TryAdd(customer.Id, customer);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Customer customer)
        {
            _customers.TryRemove(new KeyValuePair<Guid, Customer>(customer.Id, customer));
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Customer>> (_customers.Values.ToList());
        }

        public Task<IReadOnlyList<Customer>> GetAllByUserIdAsync(Guid userId)
        {
            var customers = _customers.Where(c => c.Value.UserId == userId && c.Value.Active);
            return Task.FromResult<IReadOnlyList<Customer>>(customers.Select(c => c.Value).ToList());
        }

        public Task<Customer> GetAsync(Guid id)
        {
            _customers.TryGetValue(id, out var customer);
            return Task.FromResult<Customer>(customer);
        }

        public Task<Customer> GetDetailsAsync(Guid id)
        {
            _customers.TryGetValue(id, out var customer);
            return Task.FromResult<Customer>(customer);
        }

        public Task UpdateAsync(Customer customer)
        {
            return Task.CompletedTask;
        }
    }
}
