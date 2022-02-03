using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal class Rate
    {
        public string No { get; set; }
        public DateOnly EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
}
