using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class OrderCannotBeDeletedException : ECommerceException
    {
        public Guid OrderId { get; }

        public OrderCannotBeDeletedException(Guid orderId) : base($"Order with id '{orderId}' cannot be deleted.")
        {
            OrderId = orderId;
        }
    }
}
