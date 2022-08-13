using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Sales.Application.Orders.Queries
{
    public record GetAllMyItemsInCart(Guid UserId) : IQuery<IEnumerable<OrderItemDto>>;
}
