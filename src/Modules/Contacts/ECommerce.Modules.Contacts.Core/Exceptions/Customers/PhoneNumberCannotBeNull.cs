using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class PhoneNumberCannotBeNull : ECommerceException
    {
        public PhoneNumberCannotBeNull() : base("PhoneNumber cannot be null.")
        {
        }
    }
}
