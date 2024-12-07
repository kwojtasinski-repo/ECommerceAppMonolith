using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class ItemSaleRepository(ItemsDbContext dbContext) : IItemSaleRepository
    {
        public async Task AddAsync(ItemSale itemSale)
        {
            await dbContext.ItemSales.AddAsync(itemSale);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemSale itemSale)
        {
            dbContext.ItemSales.Remove(itemSale);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ItemSale>> GetAllAsync()
        {
            return await dbContext.ItemSales.Include(i => i.Item)
                                                      .ToListAsync();
        }

        public async Task<ItemSale?> GetAsync(Guid id)
        {
            return await dbContext.ItemSales.Include(i => i.Item)
                                               .Include(i => i.Item).ThenInclude(b => b.Brand)
                                               .Include(i => i.Item).ThenInclude(b => b.Type)
                                               .Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ItemSale itemSale)
        {
            dbContext.ItemSales.Update(itemSale);
            await dbContext.SaveChangesAsync();
        }
    }
}
