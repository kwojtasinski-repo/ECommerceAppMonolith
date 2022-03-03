using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    public class ItemCartCannotBeNullException : ECommerceException
    {
        public ItemCartCannotBeNullException() : base("Item cannot be null.")
        {
        }
    }
}
