namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class UserCustomersMap : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "userCustomersMap";

        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
    }
}
