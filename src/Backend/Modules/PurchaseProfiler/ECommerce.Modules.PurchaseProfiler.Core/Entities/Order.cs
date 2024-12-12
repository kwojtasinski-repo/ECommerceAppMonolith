namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class Order : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "orders";

        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalCost { get; set; }
        public List<OrderItem> Items { get; set; } = [];
    }

    public class OrderItem
    {
        public Guid ItemId { get; set; }
        public decimal Cost { get; set; }
        // other fields?
    }
}
