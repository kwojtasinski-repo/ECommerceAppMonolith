using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class ItemSaleRepository : IItemSaleRepository
    {
        private readonly ItemsDbContext _dbContext;

        public ItemSaleRepository(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ItemSale itemSale)
        {
            await _dbContext.ItemSales.AddAsync(itemSale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemSale itemSale)
        {
            _dbContext.ItemSales.Remove(itemSale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ItemSale>> GetAllAsync()
        {
            var itemSales = await _dbContext.ItemSales.Include(i => i.Item)
                                                      .ToListAsync();
            return itemSales;
        }

        public async Task<ItemSale> GetAsync(Guid id)
        {
            var itemSale = await _dbContext.ItemSales.Include(i => i.Item)
                                               .Include(i => i.Item).ThenInclude(b => b.Brand)
                                               .Include(i => i.Item).ThenInclude(b => b.Type)
                                               .Where(i => i.Id == id).SingleOrDefaultAsync();
            return itemSale;
        }

        public async Task UpdateAsync(ItemSale itemSale)
        {
            _dbContext.ItemSales.Update(itemSale);
            await _dbContext.SaveChangesAsync();
        }
    }
}
