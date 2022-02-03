using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class RateNotFoundException : ECommerceException
    {
        public string CurrrencyCode { get; }
        public DateOnly Date { get; }

        public RateNotFoundException(string currencyCode, DateOnly date) : base($"Rate for currency '{currencyCode}' and date '{date}' was not found.")
        {
            CurrrencyCode = currencyCode;
            Date = date;
        }
    }
}
