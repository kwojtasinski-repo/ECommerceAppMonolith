using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Orders.Events
{
    public class OrderPaid : IEvent
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreateOrderDate { get; set; }
        public DateTime OrderApprovedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<OrderPaidItem> OrderItems { get; set; } = [];
    }

    public class OrderPaidItem
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public Guid UserId { get; set; }
    }

    public static class Extension
    {
        public static OrderPaid AsOrderPaid(this Order order, DateTime paymentDate)
        {
            return order is null
                ? throw new ArgumentNullException(nameof(order))
                : new OrderPaid
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    CreateOrderDate = order.CreateOrderDate,
                    OrderApprovedDate = order.OrderApprovedDate ?? new DateTime(),
                    Price = order.Price.Value,
                    Rate = order.Currency?.Rate ?? 1,
                    PaymentDate = paymentDate,
                    CurrencyCode = order.Currency?.CurrencyCode ?? string.Empty,
                    CustomerId = order.CustomerId,
                    UserId = order.UserId,
                    OrderItems = order.OrderItems?.Select(oi => new OrderPaidItem
                    {
                        Id = oi.Id,
                        CurrencyCode = oi.Currency?.CurrencyCode ?? string.Empty,
                        Rate = oi.Currency?.Rate ?? 1,
                        Price = oi.Price.Value,
                        UserId = oi.UserId,
                        ItemId = oi.ItemCart?.ItemId ?? Guid.Empty
                    }) ?? []
                };
            }
    }
}
