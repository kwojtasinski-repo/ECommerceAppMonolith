using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    internal class Image
    {
        public Guid Id { get; set; }
        public string SourcePath { get; set; }
        public string ImageName { get; set; }
        public bool? MainImage { get; set; }
        public Item? Item { get; set; }
        public Guid? ItemId { get; set; }
    }
}
