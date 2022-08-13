using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class OrderItemCannotBeDeletedException : ECommerceException
    {
        public Guid OrderItemId { get; }

        public OrderItemCannotBeDeletedException(Guid orderItemId) : base($"OrderItem with id: '{orderItemId}' cannot be deleted")
        {
            OrderItemId = orderItemId;
        }
    }
}
