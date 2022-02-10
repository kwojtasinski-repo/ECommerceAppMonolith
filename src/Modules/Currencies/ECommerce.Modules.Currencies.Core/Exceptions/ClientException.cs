using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    internal class ClientException : ECommerceException
    {
        public string Url { get; }
        public int StatusCode { get; }

        public ClientException(string url, int statusCode) : base($"Client requested address '{url}' has response with status '{statusCode}'. Please check your url and verify integration.")
        {
            Url = url;
            StatusCode = statusCode;
        }
    }
}
