using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class StreetNameTooSmallException : ECommerceException
    {
        public string StreetName { get; }

        public StreetNameTooSmallException(string streetName) : base($"StreetName '{streetName}' is too small.")
        {
            StreetName = streetName;
        }
    }
}
