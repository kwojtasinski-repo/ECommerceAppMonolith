using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class CompanyNameTooLongException : ECommerceException
    {
        public string CompanyName { get; }

        public CompanyNameTooLongException(string companyName) : base($"CompanyName '{companyName}' is too long.")
        {
            CompanyName = companyName;
        }
    }
}
