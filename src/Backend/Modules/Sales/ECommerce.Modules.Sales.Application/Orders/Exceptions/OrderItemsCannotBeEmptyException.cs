using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class OrderItemsCannotBeEmptyException : ECommerceException
    {
        public OrderItemsCannotBeEmptyException() : base("OrderItems cannot be empty.")
        {
        }
    }
}
