namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class User : DocumentEntity, IDocumentEntity<long>
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
    }
}
