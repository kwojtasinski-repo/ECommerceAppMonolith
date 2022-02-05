using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class ImageNameCannotBeNullException : ECommerceException
    {
        public ImageNameCannotBeNullException() : base("ImageName cannot be empty.")
        {
        }
    }
}
