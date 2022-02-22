using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal sealed class ItemRepository : IItemRepository
    {
        public Task AddAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
