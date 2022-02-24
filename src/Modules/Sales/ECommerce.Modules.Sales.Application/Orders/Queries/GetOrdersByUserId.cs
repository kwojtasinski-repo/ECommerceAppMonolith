using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Sales.Application.Orders.Queries
{
    public class GetOrdersByUserId : IQuery<IEnumerable<OrderDto>>
    {
        public Guid UserId { get; set; }
    }
}
