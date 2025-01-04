using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class ProductRepository
        (
            IGenericRepository<Product, long> genericRepository,
            ILogger<ProductRepository> logger
        )
        : IProductRepository
    {
        private readonly string _collectionName = genericRepository.CollectionName;

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

        public async Task<Product?> GetByProductSaleIdAsync(Guid productSaleId)
        {
            var query = string.Format("FOR product IN {0} FILTER product.ProductSaleId == @productSaleId RETURN product", _collectionName);
            var bindVars = new Dictionary<string, object> { { "productSaleId", productSaleId } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<Product>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collection '{collection}' with productSaleId '{productSaleId}', status code: '{statusCode}'", _collectionName, productSaleId, (int)(response?.Code ?? 0));
                return null;
            }

            return response.Result.FirstOrDefault();
        }

        public async Task<List<Product>> GetProductsByItemsIdsAsync(IEnumerable<Guid> itemsId)
        {
            var itemsIdsString = itemsId.Select(i => i.ToString());
            var query = string.Format("FOR product IN {0} FILTER product.ProductId IN @itemsId RETURN product", _collectionName);
            var bindVars = new Dictionary<string, object> { { "itemsId", itemsIdsString } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<Product>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collection '{collection}' with productSaleId '{productSaleId}', status code: '{statusCode}'", _collectionName, string.Join(',', itemsIdsString), (int)(response?.Code ?? 0));
                return [];
            }
            return response.Result.ToList();
        }

        public async Task<List<Guid>> GetProductsIdsByKeysAsync(IEnumerable<long> predictedProductsKeys)
        {
            var query = string.Format("FOR product IN {0} FILTER product._key IN @itemsKey RETURN product.ProductId", _collectionName);
            var bindVars = new Dictionary<string, object> { { "itemsKey", predictedProductsKeys.Select(k => k.ToString()) } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<Guid>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting productsId for '{collection}' with productSaleKeys '{productSaleKeys}', status code: '{statusCode}'", _collectionName, string.Join(',', predictedProductsKeys), (int)(response?.Code ?? 0));
                return [];
            }
            return response.Result.ToList();
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            return await genericRepository.UpdateAsync(product);
        }
    }
}
