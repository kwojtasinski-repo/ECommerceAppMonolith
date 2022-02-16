using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class Item
    {
        public Guid Id { get; private set; }
        public string ItemName { get; private set; }
        public string BrandName { get; private set; }
        public string TypeName { get; private set; }
        public string? Description { get; private set; }
        public decimal Cost { get; private set; }
        public IEnumerable<string>? Tags { get; private set; }
        public IEnumerable<ImageUrl> ImagesUrl { get; private set; }

        public Item(Guid id, string itemName, string brandName, string typeName, string? description, decimal cost, IEnumerable<string>? tags, IEnumerable<ImageUrl> imageUrls)
        {
            Id = id;
            ItemName = itemName;
            BrandName = brandName;
            TypeName = typeName;
            Description = description;
            Cost = cost;
            Tags = tags;
            ImagesUrl = imageUrls;
        }
    }
}
