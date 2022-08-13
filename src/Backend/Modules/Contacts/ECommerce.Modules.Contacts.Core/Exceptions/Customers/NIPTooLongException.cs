using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class NIPTooLongException : ECommerceException
    {
        public string NIP { get; set; }

        public NIPTooLongException(string nip) : base($"NIP '{nip}' is too long.")
        {
            NIP = nip;
        }
    }
}
