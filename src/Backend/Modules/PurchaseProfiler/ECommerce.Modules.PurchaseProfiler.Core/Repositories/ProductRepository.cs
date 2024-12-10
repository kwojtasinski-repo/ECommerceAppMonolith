using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class ProductRepository(IGenericRepository<Product, long> genericRepository)
        : IProductRepository
    {
        public async Task<Product> AddAsync(Product product)
        {
            return await genericRepository.AddAsync(product);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<Product?> GetByKeyAsync(string key)
        {
            return await genericRepository.GetByKeyAsync(key);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            return await genericRepository.UpdateAsync(product);
        }
    }
}
