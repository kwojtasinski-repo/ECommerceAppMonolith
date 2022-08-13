using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.Exceptions
{
    internal class InvalidPasswordException : ECommerceException
    {
        public InvalidPasswordException() : base("Password should have at least eight characters, one uppercase letter, one lowercase letter and one number")
        {
        }
    }
}
