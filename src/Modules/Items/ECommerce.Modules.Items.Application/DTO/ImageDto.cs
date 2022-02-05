using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.DTO
{
    internal class ImageDto
    {
        public Guid Id { get; set; }
        public Guid? ItemId { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
    }
}
