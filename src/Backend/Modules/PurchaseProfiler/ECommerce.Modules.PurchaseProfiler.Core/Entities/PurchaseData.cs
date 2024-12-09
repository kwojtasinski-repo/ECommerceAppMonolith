namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class PurchaseData : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "purchaseDatas";

        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public float Price { get; set; }
        public float PurchaseFrequency { get; set; }
        public bool PurchasedProduct { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
