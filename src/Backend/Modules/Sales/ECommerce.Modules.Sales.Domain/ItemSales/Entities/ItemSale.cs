namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class ItemSale
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Guid ItemId { get; set; }
        public decimal Cost { get; set; }
        public string CurrencyCode { get; set; }
        public bool Active { get; set; }

        private ItemSale()
        {
        }

        public ItemSale(Guid id, Item item, decimal cost, string currencyCode, bool active = true)
        {
            Id = id;
            Item = item;
            ItemId = item.Id;
            Cost = cost;
            CurrencyCode = currencyCode;
            Active = active;
        }
    }
}