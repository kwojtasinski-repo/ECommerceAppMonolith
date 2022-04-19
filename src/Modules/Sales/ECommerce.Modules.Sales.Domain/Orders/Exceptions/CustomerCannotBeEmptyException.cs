using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class CustomerCannotBeEmptyException : ECommerceException
    {
        public CustomerCannotBeEmptyException() : base("Customer cannot be empty")
        {
        }
    }
}
