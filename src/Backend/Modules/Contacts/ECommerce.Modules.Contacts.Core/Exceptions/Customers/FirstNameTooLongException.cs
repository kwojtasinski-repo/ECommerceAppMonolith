using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class FirstNameTooLongException : ECommerceException
    {
        public string FirstName { get; set; }

        public FirstNameTooLongException(string firstName) : base($"FirstName '{firstName}' is too long.")
        {
            FirstName = firstName;
        }
    }
}
