using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class CannotUpdateItemException : ECommerceException
    {
        public Guid ItemId { get; }

        public CannotUpdateItemException(Guid itemId) : base($"Item with id: '{itemId}' cannot be updated.")
        {
            ItemId = itemId;
        }
    }
}
