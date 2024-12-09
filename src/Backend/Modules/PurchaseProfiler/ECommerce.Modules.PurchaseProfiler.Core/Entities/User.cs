namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class User : DocumentEntity, IDocumentEntity<long>
    {
        public User(string? id = null, string? key = null)
            : base(id, key)
        { }

        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
    }
}
