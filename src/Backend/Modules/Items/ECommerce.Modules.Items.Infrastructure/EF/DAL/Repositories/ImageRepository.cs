using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories
{
    internal sealed class ImageRepository : IImageRepository
    {
        private readonly ItemsDbContext _dbContext;

        public ImageRepository(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Image image)
        {
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Image image)
        {
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Image>> GetAllAsync()
        {
            var images = await _dbContext.Images.ToListAsync();
            return images;
        }

        public async Task<Image> GetAsync(Guid id)
        {
            var image = await _dbContext.Images.Where(i => i.Id == id).SingleOrDefaultAsync();
            return image;
        }

        public async Task UpdateAsync(Image image)
        {
            _dbContext.Images.Update(image);
            await _dbContext.SaveChangesAsync();
        }
    }
}
