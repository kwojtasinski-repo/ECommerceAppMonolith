using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    public class CurrencyCannotBeChangedInOrderException : ECommerceException
    {
        public Guid OrderId { get; }

        public CurrencyCannotBeChangedInOrderException(Guid orderId) : base($"Currency cannot be changed in Order with id '{orderId}'")
        {
            OrderId = orderId;
        }
    }
}
