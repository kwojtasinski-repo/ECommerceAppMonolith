using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class ServerNotAvailableException : ECommerceException
    {
        public string Url { get; set; }

        public ServerNotAvailableException(string url) : base($"Server at address '{url}' is not available.")
        {
            Url = url;
        }
    }
}
