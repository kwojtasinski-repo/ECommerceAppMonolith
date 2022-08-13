using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public BrandDto Brand { get; set; }
        public TypeDto Type { get; set; }
        public ImageUrl ImagesUrl { get; set; }
    }
}
