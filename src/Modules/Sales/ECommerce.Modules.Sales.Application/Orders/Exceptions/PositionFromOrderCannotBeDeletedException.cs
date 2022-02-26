using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class PositionFromOrderCannotBeDeletedException : ECommerceException
    {
        public Guid OrderId { get; }
        public Guid OrderItemId { get; }

        public PositionFromOrderCannotBeDeletedException(Guid orderId, Guid orderItemId) : base($"Position OrderItem with id '{orderItemId}' cannot be deleted from Order with id '{orderId}'.")
        {
            OrderId = orderId;
            OrderItemId = orderItemId;
        }
    }
}
