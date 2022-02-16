using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class OrderCostCannotBeNegativeException : ECommerceException
    {
        public decimal Cost { get; }

        public OrderCostCannotBeNegativeException(decimal cost) : base($"OrderCost '{cost}' cannot be negative.")
        {
            Cost = cost;
        }
    }
}
