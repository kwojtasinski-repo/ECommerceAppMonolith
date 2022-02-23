using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class OrderItemNotFoundException : ECommerceException
    {
        public Guid OrderId { get; }
        public Guid OrderItemId { get; }

        public OrderItemNotFoundException(Guid orderId, Guid orderItemId) : base($"Order with id: '{orderId}' doesnt have OrderItem with id: '{orderItemId}'.")
        {
            OrderId = orderId;
            OrderItemId = orderItemId;
        }
    }
}
