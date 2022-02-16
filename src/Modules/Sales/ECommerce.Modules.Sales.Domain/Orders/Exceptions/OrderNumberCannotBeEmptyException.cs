using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class OrderNumberCannotBeEmptyException : ECommerceException
    {
        public OrderNumberCannotBeEmptyException() : base("OrderNumber cannot be empty.")
        {
        }
    }
}
