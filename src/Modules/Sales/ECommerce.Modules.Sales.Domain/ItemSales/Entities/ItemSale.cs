namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class ItemSale
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Guid ItemId { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }

        public ItemSale(Guid id, Item item, decimal cost, bool active = true)
        {
            Id = id;
            Item = item;
            ItemId = item.Id;
            Cost = cost;
            Active = active;
        }
    }
}