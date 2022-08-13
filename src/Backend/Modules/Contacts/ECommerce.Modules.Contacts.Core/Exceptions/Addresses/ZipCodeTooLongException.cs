using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class ZipCodeTooLongException : ECommerceException
    {
        public string ZipCode { get; }

        public ZipCodeTooLongException(string zipCode) : base($"ZipCode '{zipCode}' is too long.")
        {
            ZipCode = zipCode;
        }
    }
}
