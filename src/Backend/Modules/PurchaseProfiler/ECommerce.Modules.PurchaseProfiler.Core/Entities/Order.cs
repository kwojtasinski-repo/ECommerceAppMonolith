namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalCost { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string ItemId { get; set; }
        public decimal Cost { get; set; }
        // other fields?
    }
}
