using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class PhoneNumberTooShortException : ECommerceException
    {
        public string PhoneNumber { get; }

        public PhoneNumberTooShortException(string phoneNumber) : base($"PhoneNumber '{phoneNumber}' is too short.")
        {
            PhoneNumber = phoneNumber;
        }
    }
}
