using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class CountryNameTooLongException : ECommerceException
    {
        public string CountryName { get; }

        public CountryNameTooLongException(string countryName) : base($"CountryName '{countryName}' is too long.")
        {
            CountryName = countryName;
        }
    }
}
