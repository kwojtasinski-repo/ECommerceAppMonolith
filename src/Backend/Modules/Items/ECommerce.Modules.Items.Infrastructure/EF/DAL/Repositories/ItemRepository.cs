using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class ItemRepository : IItemRepository
    {
        private readonly ItemsDbContext _dbContext;

        public ItemRepository(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Item>> GetAllAsync()
        {
            var items = await _dbContext.Items.Include(b => b.Brand)
                                              .Include(t => t.Type)
                                              .ToListAsync();
            return items;
        }

        public async Task<Item?> GetAsync(Guid id)
        {
            return await _dbContext.Items.Include(b => b.Brand)
                                             .Include(t => t.Type)
                                             .Include(i => i.ItemSale)
                                             .Where(i => i.Id == id)
                                             .FirstOrDefaultAsync();
        }

        public async Task<Item?> GetProductDataAsync(Guid id)
        {
            return await _dbContext.Items.Include(b => b.Brand)
                                             .Include(t => t.Type)
                                             .Include(i => i.ItemSale)
                                             .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateAsync(Item item)
        {
            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
