using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
        public string? Description { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public IEnumerable<string>? ImagesUrl { get; set; }
        public ItemSale? ItemSale { get; set; }

        public Item(Guid id, string itemName, string brandName, string typeName, string? description, IEnumerable<string>? tags, IEnumerable<string>? imagesUrl)
        {
            Id = id;
            ItemName = itemName;
            BrandName = brandName;
            TypeName = typeName;
            Description = description;
            Tags = tags;
            ImagesUrl = imagesUrl;
        }
    }
}
