using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemSaleDto
    {
        public Guid Id { get; set; }
        public ItemDto Item { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }
    }
}
