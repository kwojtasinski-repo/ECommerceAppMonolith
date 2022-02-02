using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.DTO
{
    internal class CurrencyRateDto
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime CurrencyDate { get; set; }
    }
}
