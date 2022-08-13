using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class LocaleNumberTooLongException : ECommerceException
    {
        public string LocaleNumber { get; }

        public LocaleNumberTooLongException(string localeNumber) : base($"LocaleNumber '{localeNumber}' is too long.")
        {
            LocaleNumber = localeNumber;
        }
    }
}
