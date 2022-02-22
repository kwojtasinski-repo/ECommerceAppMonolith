using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public decimal Cost { get; private set; }
        public bool Active { get; private set; }

        public ItemCart(Guid id, string itemName, string brandName, string typeName, string description, IEnumerable<string>? tags, IEnumerable<string> imageUrls,
            decimal cost, bool active = true)
        {
            Id = id;
            ItemName = itemName;
            BrandName = brandName;
            TypeName = typeName;
            Description = description;
            Tags = tags;
            ImagesUrl = imageUrls;
            Cost = cost;
            Active = active;
        }
    }
}
