using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    internal class Item
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public Brand Brand { get; set; }
        public Guid BrandId { get; set; }
        public Type Type { get; set; }
        public Guid TypeId { get; set; }

        public Dictionary<string, IEnumerable<string>>? Tags { get; set; }
        public Dictionary<string, IEnumerable<string>>? ImagesUrl { get; set; }

        public ICollection<Image>? Images { get; set; }
    }
}
