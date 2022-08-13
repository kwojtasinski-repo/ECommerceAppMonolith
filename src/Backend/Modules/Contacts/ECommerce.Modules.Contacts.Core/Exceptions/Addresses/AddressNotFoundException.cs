using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class AddressNotFoundException : ECommerceException
    {
        public Guid AddressId { get; set; }

        public AddressNotFoundException(Guid addressId) : base($"Address with id '{addressId}' was not found.")
        {
            AddressId = addressId;
        }
    }
}
