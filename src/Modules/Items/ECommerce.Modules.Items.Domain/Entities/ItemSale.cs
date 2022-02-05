using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    internal class ItemSale
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Guid ItemId { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }
    }
}
