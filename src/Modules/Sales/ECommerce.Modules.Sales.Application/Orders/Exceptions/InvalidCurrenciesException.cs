using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class InvalidCurrenciesException : ECommerceException
    {
        public IEnumerable<string> CurrencyCodesFound { get; }
        public IEnumerable<string> CurrencyCodesNotFound { get; }

        public InvalidCurrenciesException(IEnumerable<string> currencyCodesFound, IEnumerable<string> currencyCodesNotFound) : base($"Invalid currencies. Found '{currencyCodesFound}' and not found '{currencyCodesNotFound}'")
        {
            CurrencyCodesFound = currencyCodesFound;
            CurrencyCodesNotFound = currencyCodesNotFound;
        }
    }
}
