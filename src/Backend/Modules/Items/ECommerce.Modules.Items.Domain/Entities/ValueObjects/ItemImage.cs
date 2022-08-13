using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities.ValueObjects
{
    public class ItemImage
    {
        public string Url { get; set; }
        public bool MainImage { get; set; } = false;
    }
}
