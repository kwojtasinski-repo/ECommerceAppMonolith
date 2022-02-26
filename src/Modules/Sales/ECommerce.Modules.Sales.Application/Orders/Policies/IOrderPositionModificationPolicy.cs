using ECommerce.Modules.Sales.Domain.Orders.Entities;

namespace ECommerce.Modules.Sales.Application.Orders.Policies
{
    public interface IOrderPositionModificationPolicy
    {
        Task<bool> CanAddAsync(Order order);
        Task<bool> CanDeleteAsync(Order order);
    }
}
