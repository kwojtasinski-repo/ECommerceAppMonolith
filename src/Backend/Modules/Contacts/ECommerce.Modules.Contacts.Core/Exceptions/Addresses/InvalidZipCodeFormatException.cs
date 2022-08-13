using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class InvalidZipCodeFormatException : ECommerceException
    {
        public string ZipCode { get; }

        public InvalidZipCodeFormatException(string zipCode) : base($"Invalid ZipCode format '{zipCode}'.")
        {
            ZipCode = zipCode;
        }
    }
}
