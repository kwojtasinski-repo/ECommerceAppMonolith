using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class PhoneNumberTooLongException : ECommerceException
    {
        public string PhoneNumber { get; }

        public PhoneNumberTooLongException(string phoneNumber) : base($"PhoneNumber '{phoneNumber}' is too long.")
        {
            PhoneNumber = phoneNumber;
        }
    }
}
