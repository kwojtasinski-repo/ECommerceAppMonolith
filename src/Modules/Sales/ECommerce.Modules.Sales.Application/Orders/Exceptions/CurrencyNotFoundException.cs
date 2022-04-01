using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class CurrencyNotFoundException : ECommerceException
    {
        public string CurrencyCode { get; }
        public DateOnly CurrencyDate { get; }

        public CurrencyNotFoundException(string currencyCode, DateOnly currencyDate) : base($"Currency '{currencyCode}' not found for '{currencyDate}'")
        {
            CurrencyCode = currencyCode;
            CurrencyDate = currencyDate;
        }
    }
}
