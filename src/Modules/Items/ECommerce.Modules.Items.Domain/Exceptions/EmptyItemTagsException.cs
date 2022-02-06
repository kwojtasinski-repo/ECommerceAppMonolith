using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class EmptyItemTagsException : ECommerceException
    {
        public Guid ItemId { get; }

        public EmptyItemTagsException(Guid itemId)
            : base($"Item with id: '{itemId}' defines empty tags.")
            => ItemId = itemId;
    }
}
