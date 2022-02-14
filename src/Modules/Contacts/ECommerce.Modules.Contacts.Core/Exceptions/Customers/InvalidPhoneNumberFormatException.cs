using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class InvalidPhoneNumberFormatException : ECommerceException
    {
        public string PhoneNumber { get; }

        public InvalidPhoneNumberFormatException(string phoneNumber) : base($"Invalid PhoneNumber '{phoneNumber}'. PhoneNumber optional can take string starts with '+' and digits length must be beetween 7 and 16.")
        {
            PhoneNumber = phoneNumber;
        }
    }
}
