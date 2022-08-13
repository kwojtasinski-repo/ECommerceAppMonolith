using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class TypeRepository : ITypeRepository
    {
        private readonly ItemsDbContext _dbContext;

        public TypeRepository(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Domain.Entities.Type type)
        {
            await _dbContext.Types.AddAsync(type);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Domain.Entities.Type type)
        {
            _dbContext.Types.Remove(type);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Domain.Entities.Type>> GetAllAsync()
        {
            var types = await _dbContext.Types.ToListAsync();
            return types;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var type = await _dbContext.Types.Where(t => t.Id == id).AsNoTracking().SingleOrDefaultAsync();
            return type != null;
        }

        public async Task<Domain.Entities.Type> GetAsync(Guid id)
        {
            var type = await _dbContext.Types.Where(t => t.Id == id).SingleOrDefaultAsync();
            return type;
        }

        public async Task<Domain.Entities.Type> GetDetailsAsync(Guid id)
        {
            var type = await _dbContext.Types.Include(i => i.Items).Where(t => t.Id == id).SingleOrDefaultAsync();
            return type;
        }

        public async Task UpdateAsync(Domain.Entities.Type type)
        {
            _dbContext.Types.Update(type);
            await _dbContext.SaveChangesAsync();
        }
    }
}
