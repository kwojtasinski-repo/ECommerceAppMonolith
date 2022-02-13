using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class CountryNameTooSmallException : ECommerceException
    {
        public string CountryName { get; }

        public CountryNameTooSmallException(string countryName) : base($"CountryName '{countryName}' is too small.")
        {
            CountryName = countryName;
        }
    }
}
