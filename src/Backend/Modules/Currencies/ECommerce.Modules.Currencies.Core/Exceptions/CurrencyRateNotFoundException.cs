using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class CurrencyRateNotFoundException : ECommerceException
    {
        public Guid CurrencyRateId { get; }

        public CurrencyRateNotFoundException(Guid id) : base($"CurrencyRate with id: '{id}' was not found.")
        {
            CurrencyRateId = id;
        }
    }
}
