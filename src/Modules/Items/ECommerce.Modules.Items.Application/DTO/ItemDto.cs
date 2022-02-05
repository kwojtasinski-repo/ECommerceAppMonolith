using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Core.Application
{
    internal class ItemDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public BrandDto Brand { get; set; }
        public TypeDto Type { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public IEnumerable<string>? ImagesUrl { get; set; }
    }
}
