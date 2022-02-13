using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class LastNameCannotBeNullException : ECommerceException
    {
        public LastNameCannotBeNullException() : base("LastName cannot be null")
        {
        }
    }
}
