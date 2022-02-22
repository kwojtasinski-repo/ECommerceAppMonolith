using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal sealed class InMemoryItemRepository : IItemRepository
    {
        private readonly ConcurrentDictionary<Guid, Item> _items = new();

        public Task AddAsync(Item item)
        {
            _items.TryAdd(item.Id, item);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item != null);
        }

        public Task<Item> GetAsync(Guid id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult<Item>(item);
        }

        public Task UpdateAsync(Item item)
        {
            return Task.CompletedTask;
        }
    }
}
