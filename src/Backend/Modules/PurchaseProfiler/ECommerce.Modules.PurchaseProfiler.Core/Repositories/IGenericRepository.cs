using ArangoDBNetStandard;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal interface IGenericRepository<T, U>
        where T : class, IDocumentEntity<U>
        where U : struct
    {
        string CollectionName { get; }
        public IArangoDBClient DbClient { get; }
        Task<T?> GetByKeyAsync(string key);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string key);
    }
}
