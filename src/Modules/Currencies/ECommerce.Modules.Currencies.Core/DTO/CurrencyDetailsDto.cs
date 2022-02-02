using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.DTO
{
    internal class CurrencyDetailsDto : CurrencyDto
    {
        public List<CurrencyRateDto> CurrencyRates { get; set; }
    }
}
