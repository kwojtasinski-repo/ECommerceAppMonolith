﻿using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByKeyAsync(string key);
        Task<Order?> GetByOrderIdAsync(Guid orderId);
        Task<Order> AddAsync(Order order);
        Task<Order?> UpdateAsync(Order order);
        Task<bool> DeleteAsync(string key);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<Order>> GetOrdersByUserIdAndOrderDateRangeAsync(Guid userId, DateTime orderDateStart, DateTime orderDateEnd);
    }
}
