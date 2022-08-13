using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class StreetNameTooLongException : ECommerceException
    {
        public string StreetName { get; }

        public StreetNameTooLongException(string streetName) : base($"StreeName '{streetName}' is too long.")
        {
            StreetName = streetName;
        }
    }
}
