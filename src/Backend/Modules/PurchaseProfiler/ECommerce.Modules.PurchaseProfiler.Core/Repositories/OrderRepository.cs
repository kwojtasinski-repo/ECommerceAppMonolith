using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class OrderRepository(IGenericRepository<Order, long> genericRepository)
        : IOrderRepository
    {
        public async Task<Order> AddAsync(Order order)
        {
            return await genericRepository.AddAsync(order);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<Order?> GetByKeyAsync(string key)
        {
            return await genericRepository.GetByKeyAsync(key);
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            return await genericRepository.UpdateAsync(order);
        }
    }
}
