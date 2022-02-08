using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class ItemImagesNotAllowedMoreThanOneMainImageException : ECommerceException
    {
        public ItemImagesNotAllowedMoreThanOneMainImageException() : base("ItemImages not allowed more than one main images.")
        {
        }
    }
}
