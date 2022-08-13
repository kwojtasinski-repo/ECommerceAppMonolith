using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class CurrencyCodeCannotBeNullException : ECommerceException
    {
        public CurrencyCodeCannotBeNullException() : base("CurrencyCode cannot be null.")
        {
        }
    }
}
