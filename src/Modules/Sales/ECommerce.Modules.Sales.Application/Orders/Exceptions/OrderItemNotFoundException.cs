using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class OrderItemNotFoundException : ECommerceException
    {
        public Guid OrderItemId { get; }

        public OrderItemNotFoundException(Guid orderItemId) : base($"OrderItem with id '{orderItemId}' was not found.")
        {
            OrderItemId = orderItemId;
        }
    }
}
