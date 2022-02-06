using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class EmptyItemImagesUrlException : ECommerceException
    {
        public Guid ItemId { get; }

        public EmptyItemImagesUrlException(Guid itemId)
            : base($"Item with id: '{itemId}' defines empty imagesUrl.")
            => ItemId = itemId;
    }
}
