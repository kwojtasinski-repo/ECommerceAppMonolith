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
    internal sealed class InMemoryItemSaleRepository : IItemSaleRepository
    {
        private readonly ConcurrentDictionary<Guid, ItemSale> _itemSales = new();

        public Task AddAsync(ItemSale itemSale)
        {
            _itemSales.TryAdd(itemSale.Id, itemSale);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            _itemSales.TryGetValue(id, out var itemSale);
            return Task.FromResult(itemSale != null);
        }

        public Task<ItemSale> GetAsync(Guid id)
        {
            _itemSales.TryGetValue(id, out var itemSale);
            return Task.FromResult<ItemSale>(itemSale);
        }

        public Task UpdateAsync(ItemSale itemSale)
        {
            return Task.CompletedTask;
        }
    }
}
