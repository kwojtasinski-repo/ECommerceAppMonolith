using ECommerce.Modules.Sales.Domain.Orders.Entities;

namespace ECommerce.Modules.Sales.Domain.Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetAsync(Guid id);
        Task<Order?> GetDetailsAsync(Guid id);
        Task UpdateAsync(Order order);
        Task AddAsync(Order order);
        Task DeleteAsync(Order order);
        Task<Order?> GetLatestOrderOnDateAsync(DateTime dateTime);
    }
}
