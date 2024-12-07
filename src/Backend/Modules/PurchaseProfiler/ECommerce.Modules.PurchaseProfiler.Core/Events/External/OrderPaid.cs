using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External
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
}
