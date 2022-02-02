using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class CurrencyNotFoundException : ECommerceException
    {
        public CurrencyNotFoundException(Guid id) : base($"Currency with id: '{id}' was not found.")
        {
        }
    }
}
