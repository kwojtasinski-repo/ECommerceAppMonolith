using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Payments.Exceptions
{
    internal class OrderNotFoundException : ECommerceException
    {
        public Guid OrderId { get; }

        public OrderNotFoundException(Guid orderId) : base($"Order with id '{orderId}' was not found.")
        {
            OrderId = orderId;
        }
    }
}
