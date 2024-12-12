namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class User : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "users";

        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
