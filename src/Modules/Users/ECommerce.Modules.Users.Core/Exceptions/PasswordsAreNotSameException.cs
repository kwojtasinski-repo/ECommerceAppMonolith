using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.Exceptions
{
    internal class PasswordsAreNotSameException : ECommerceException
    {
        public PasswordsAreNotSameException() : base("Passwords are not same")
        {
        }
    }
}
