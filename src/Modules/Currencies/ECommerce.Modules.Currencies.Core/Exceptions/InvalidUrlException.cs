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
        public string Url { get; }

        public InvalidUrlException(string url) : base($"Given invalid url. Check '{url}' if is correct.")
        {
            Url = url;
        }
    }
}
