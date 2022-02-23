using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal sealed class ItemRepository : IItemRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public ItemRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(Item item)
        {
            await _salesDbContext.Items.AddAsync(item);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var item = await _salesDbContext.Items.Where(i => i.Id == id).AsNoTracking().SingleOrDefaultAsync();
            return item is not null;
        }

        public Task<Item> GetAsync(Guid id)
        {
            var item = _salesDbContext.Items.Where(i => i.Id == id).AsNoTracking().SingleOrDefaultAsync();
            return item;
        }

        public async Task UpdateAsync(Item item)
        {
            _salesDbContext.Items.Update(item);
            await _salesDbContext.SaveChangesAsync();
        }
    }
}
