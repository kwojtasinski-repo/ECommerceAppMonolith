using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class InvalidUrlException : ECommerceException
    {
        public InvalidUrlException() : base("Given invalid url. Check appsettings.")
        { 
        }
        
        public InvalidUrlException(Exception exception) : base("Given invalid url. Check appsettings.", exception)
        { 
        }
    }
}
