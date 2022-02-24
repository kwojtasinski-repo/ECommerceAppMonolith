using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Sales.Application.Orders.Queries
{
    public class GetOrderItem : IQuery<OrderItemDto>
    {
        public Guid OrderItemId { get; set; }
    }
}
