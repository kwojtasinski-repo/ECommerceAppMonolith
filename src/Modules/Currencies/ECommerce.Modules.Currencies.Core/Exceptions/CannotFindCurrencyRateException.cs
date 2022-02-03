using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class CannotFindCurrencyRateException : ECommerceException
    {
        public Guid CurrencyId { get; }
        public string CurrencyCode { get; }

        public CannotFindCurrencyRateException(Guid currencyId, string currencyCode) : base($"Cannot find CurrencyRate for currency id '{currencyId}' and currency code '{currencyCode}'.")
        {
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
        }
    }
}
