using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemDetailsDto : ItemDto
    {
        public string? Description { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public IEnumerable<ImageUrl>? ImagesUrl { get; set; }
    }
}
