using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class ItemCart
    {
        public Guid Id { get; private set; }
        public string ItemName { get; private set; }
        public string BrandName { get; private set; }
        public string TypeName { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<string>? Tags { get; private set; }
        public IEnumerable<string> ImagesUrl { get; private set; }
        public Money Price { get; private set; }
        public string CurrencyCode { get; private set; }
        public DateTime Created { get; private set; }
        public bool Active { get; private set; }
        public OrderItem? OrderItem { get; private set; }

        private ItemCart() { }

        public ItemCart(Guid id, string itemName, string brandName, string typeName, string description, IEnumerable<string>? tags, IEnumerable<string> imagesUrls,
            decimal cost, string currencyCode, DateTime created, bool active = true, OrderItem? orderItem = null)
        {
            Id = id;
            ItemName = itemName;
            BrandName = brandName;
            TypeName = typeName;
            Description = description;
            Tags = tags;
            ImagesUrl = imagesUrls;
            Price = new Money(cost);
            CurrencyCode = currencyCode;
            Created = created;
            Active = active;
            OrderItem = orderItem;
        }
    }
}
