using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class NIPTooShortException : ECommerceException
    {
        public string NIP { get; set; }

        public NIPTooShortException(string nip) : base($"NIP '{nip}' is too short.")
        {
            NIP = nip;
        }
    }
}
