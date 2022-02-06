using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class BrandRepository : IBrandRepository
    {
        private readonly ItemsDbContext _dbContext;

        public BrandRepository(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Brand brand)
        {
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Brand brand)
        {
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Brand>> GetAllAsync()
        {
            var brands = await _dbContext.Brands.ToListAsync();
            return brands;
        }

        public async Task<Brand> GetAsync(Guid id)
        {
            var brand = await _dbContext.Brands.Where(b => b.Id == id).SingleOrDefaultAsync();
            return brand;
        }

        public async Task<Brand> GetDetailsAsync(Guid id)
        {
            var brand = await _dbContext.Brands.Include(i => i.Items)
                                        .Where(b => b.Id == id)        
                                        .SingleOrDefaultAsync();
            return brand;
        }

        public async Task UpdateAsync(Brand brand)
        {
            _dbContext.Brands.Update(brand);
            await _dbContext.SaveChangesAsync();
        }
    }
}
