using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Items.Exceptions
{
    internal class ItemNotFoundException : ECommerceException
    {
        public Guid Id { get; }

        public ItemNotFoundException(Guid id) : base($"Item with id: '{id}' was not found.")
        {
            Id = id;
        }
    }
}
