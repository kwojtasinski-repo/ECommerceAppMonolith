using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class OrderApprovedDateCannotBeLessThanCreateOrderDateException : ECommerceException
    {
        public DateTime CreateOrderDate { get; }
        public DateTime OrderApprovedDate { get; }

        public OrderApprovedDateCannotBeLessThanCreateOrderDateException(DateTime orderApprovedDate, DateTime createOrderDate) : base($"Date '{orderApprovedDate}' cannot be less than create date '{createOrderDate}'")
        {
            OrderApprovedDate = orderApprovedDate;
            CreateOrderDate = createOrderDate;
        }
    }
}
