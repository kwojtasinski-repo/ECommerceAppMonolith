using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class OrderRepository
        (
            IGenericRepository<Order, long> genericRepository,
            ILogger<OrderRepository> logger
        )
        : IOrderRepository
    {
        private string CollectionName => genericRepository.CollectionName;

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

        public async Task<Order?> GetByOrderIdAsync(Guid orderId)
        {
            var query = string.Format("FOR order IN {0} FILTER order.OrderId == @orderId RETURN order", CollectionName);
            var bindVars = new Dictionary<string, object> { { "orderId", orderId } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<Order>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collection '{collection}' with orderId '{orderId}', status code: '{statusCode}'", CollectionName, orderId, (int)(response?.Code ?? 0));
                return null;
            }
            return response.Result.FirstOrDefault();
        }

        public async Task<List<Order>> GetOrdersByUserId(Guid userId)
        {
            var query = string.Format("FOR order IN {0} FILTER order.UserId == @userId RETURN order", CollectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<Order>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collections '{collection}' with userId '{userId}', status code: '{statusCode}'", CollectionName, userId, (int)(response?.Code ?? 0));
                return [];
            }
            return response.Result.ToList();
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            return await genericRepository.UpdateAsync(order);
        }
    }
}
