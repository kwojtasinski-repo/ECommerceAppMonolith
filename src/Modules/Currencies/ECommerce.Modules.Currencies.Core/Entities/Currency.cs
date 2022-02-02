using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Entities
{
    internal class Currency
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ICollection<CurrencyRate> CurrencyRates { get; set; }
    }
}
