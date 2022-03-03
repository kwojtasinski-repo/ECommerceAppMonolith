using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    public class CannotRefreshCostWhenItemCartIsNullException : ECommerceException
    {
        public Guid OrderId { get; }

        public CannotRefreshCostWhenItemCartIsNullException(Guid orderId) : base($"Cannot refresh cost for Order with id '{orderId}' when any ItemCart is null.")
        {
            OrderId = orderId;
        }
    }
}
