using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class CustomerNotFoundException : ECommerceException
    {
        public Guid CustomerId { get; set; }

        public CustomerNotFoundException(Guid customerId) : base($"Customer with id '{customerId}' was not found.")
        {
            CustomerId = customerId;
        }
    }
}
