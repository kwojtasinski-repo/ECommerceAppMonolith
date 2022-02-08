using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class ItemNotFoundException : ECommerceException
    {
        public Guid ItemId { get; }

        public ItemNotFoundException(Guid itemId) : base($"Item with id: '{itemId}' was not found.")
        {
            ItemId = itemId;
        }
    }
}
