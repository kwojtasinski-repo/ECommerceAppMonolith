namespace ECommerce.Modules.PurchaseProfiler.Api.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
