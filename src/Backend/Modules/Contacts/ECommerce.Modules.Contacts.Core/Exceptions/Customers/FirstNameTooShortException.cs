using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class FirstNameTooShortException : ECommerceException
    {
        public string FirstName { get; }

        public FirstNameTooShortException(string firstName) : base($"FirstName '{firstName}' is too short.")
        {
            FirstName = firstName;
        }
    }
}
