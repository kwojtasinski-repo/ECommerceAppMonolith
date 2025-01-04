using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByKeyAsync(string key);
        Task<Product?> GetByProductSaleIdAsync(Guid productId);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string key);
        Task<List<Product>> GetProductsByItemsIdsAsync(IEnumerable<Guid> itemsId);
        Task<List<Guid>> GetProductsIdsByKeysAsync(IEnumerable<long> predictedProductsKeys);
    }
}
