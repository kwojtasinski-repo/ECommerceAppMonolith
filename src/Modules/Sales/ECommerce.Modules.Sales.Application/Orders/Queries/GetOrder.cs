using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Sales.Application.Orders.Queries
{
    public class GetOrder : IQuery<OrderDetailsDto>
    {
        public Guid OrderId { get; set; }
    }
}
