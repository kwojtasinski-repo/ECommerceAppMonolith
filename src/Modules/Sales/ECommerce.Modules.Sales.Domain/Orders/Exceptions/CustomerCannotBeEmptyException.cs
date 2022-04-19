using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    public class CustomerCannotBeEmptyException : ECommerceException
    {
        public CustomerCannotBeEmptyException() : base("Customer cannot be empty")
        {
        }
    }
}
