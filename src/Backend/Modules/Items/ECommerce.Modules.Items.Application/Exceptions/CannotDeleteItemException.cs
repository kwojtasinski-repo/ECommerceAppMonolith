using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class CannotDeleteItemException : ECommerceException
    {
        public Guid ItemId { get; }

        public CannotDeleteItemException(Guid itemId) : base($"Item with id: '{itemId}' cannot be deleted.")
        {
            ItemId = itemId;
        }
    }
}
