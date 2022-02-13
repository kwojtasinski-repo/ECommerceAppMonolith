using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class LastNameTooLongException : ECommerceException
    {
        public string LastName { get; }

        public LastNameTooLongException(string lastName) : base($"LastName '{lastName}' is too long.")
        {
            LastName = lastName;
        }
    }
}
