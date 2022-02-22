using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal sealed class ItemRepository : IItemSaleRepository
    {
        public Task AddAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ItemSale> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ItemSale itemSale)
        {
            throw new NotImplementedException();
        }
    }
}
