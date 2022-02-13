using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class CityNameTooLongException : ECommerceException
    {
        public string CityName { get; }

        public CityNameTooLongException(string cityName) : base($"CityName '{cityName}' is too long.")
        {
            CityName = cityName;
        }
    }
}
