using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class CreateItemCannotBeNullException : ECommerceException
    {
        public CreateItemCannotBeNullException() : base("CreateItem cannot be null.")
        {
        }
    }
}
