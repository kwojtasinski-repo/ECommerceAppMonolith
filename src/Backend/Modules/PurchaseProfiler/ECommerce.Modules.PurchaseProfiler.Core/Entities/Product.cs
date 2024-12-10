namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class Product : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "products";

        public Guid ProductId { get; set; }
        public Guid ProductSaleId { get; set; }
        public decimal Cost { get; set; }
        public bool IsActivated { get; set; }
    }
}
