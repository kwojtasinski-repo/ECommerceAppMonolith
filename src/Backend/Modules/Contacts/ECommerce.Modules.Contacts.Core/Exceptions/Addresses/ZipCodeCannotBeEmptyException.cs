using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class ZipCodeCannotBeEmptyException : ECommerceException
    {
        public ZipCodeCannotBeEmptyException() : base("ZipCode cannot be empty.")
        {
        }
    }
}
