using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class UrlsCannotBeNullException : ECommerceException
    {
        public UrlsCannotBeNullException() : base("Urls cannot be null.")
        {
        }
    }
}
