using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class CustomerCannotBeChangedInOrderException : ECommerceException
    {
        public Guid OrderId { get; }

        public CustomerCannotBeChangedInOrderException(Guid orderId) : base($"Customer cannot be changed in Order with id '{orderId}'")
        {
            OrderId = orderId;
        }
    }
}
