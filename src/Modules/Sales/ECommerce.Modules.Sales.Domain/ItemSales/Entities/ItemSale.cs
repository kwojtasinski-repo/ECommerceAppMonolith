namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class ItemSale
    {
        public Guid Id { get; private set; }
        public Item Item { get; private set; }
        public decimal Cost { get; private set; }
        public bool Active { get; private set; }

        public ItemSale(Guid id, Item item, decimal cost, bool active = true)
        {
            Id = id;
            Item = item;
            Cost = cost;
            Active = active;
        }
    }
}