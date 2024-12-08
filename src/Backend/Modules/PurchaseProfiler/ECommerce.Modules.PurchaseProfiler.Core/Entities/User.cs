namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class User
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
    }
}
