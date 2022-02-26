using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal sealed class ItemSaleRepository : IItemSaleRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public ItemSaleRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(ItemSale itemSale)
        {
            var changeTracker = _salesDbContext.ChangeTracker;
            await _salesDbContext.ItemSales.AddAsync(itemSale);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var itemSale = await _salesDbContext.ItemSales.Where(i => i.Id == id).AsNoTracking().SingleOrDefaultAsync();
            return itemSale is not null;
        }

        public Task<ItemSale> GetAsync(Guid id)
        {
            var itemSale = _salesDbContext.ItemSales.Where(i => i.Id == id)
                .Include(i => i.Item).SingleOrDefaultAsync();
            return itemSale;
        }

        public async Task UpdateAsync(ItemSale itemSale)
        {
            _salesDbContext.ItemSales.Update(itemSale);
            await _salesDbContext.SaveChangesAsync();
        }
    }
}
