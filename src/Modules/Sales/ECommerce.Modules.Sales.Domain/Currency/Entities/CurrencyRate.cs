using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Currency.Entities
{
    public class CurrencyRate
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
        public string CurrencyCode { get; set; }
        public DateOnly RateDate { get; set; }
        public DateOnly Created { get; set; }
    }
}
