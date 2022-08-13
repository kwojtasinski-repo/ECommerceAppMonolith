using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class CompanyNameTooShortException : ECommerceException
    {
        public string CompanyName { get; }

        public CompanyNameTooShortException(string companyName) : base($"CompanyName '{companyName}' is too short.")
        {
            CompanyName = companyName;
        }
    }
}
