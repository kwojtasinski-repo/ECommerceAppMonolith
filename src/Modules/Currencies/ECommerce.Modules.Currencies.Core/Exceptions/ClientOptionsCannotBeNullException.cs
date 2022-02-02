using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class ClientOptionsCannotBeNullException : ECommerceException
    {
        public ClientOptionsCannotBeNullException() : base("ClientOptions cannot be null.")
        {
        }
    }
}