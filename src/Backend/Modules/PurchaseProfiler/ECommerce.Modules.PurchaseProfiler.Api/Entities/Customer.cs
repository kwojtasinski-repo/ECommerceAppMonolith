namespace ECommerce.Modules.PurchaseProfiler.Api.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Guid> PurchasedItemIds { get; set; }
    }
}
