namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public decimal Cost { get; set; }
        public bool IsActivated { get; set; }
    }
}
