using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    internal class Type
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
