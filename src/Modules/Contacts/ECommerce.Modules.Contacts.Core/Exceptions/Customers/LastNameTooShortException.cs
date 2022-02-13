using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class LastNameTooShortException : ECommerceException
    {
        public string LastName { get; }

        public LastNameTooShortException(string lastName) : base($"LastName '{lastName}' is too short.")
        {
            LastName = lastName;
        }
    }
}
