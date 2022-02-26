using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class PositionToOrderCannotBeAddedException : ECommerceException
    {
        public Guid OrderId { get; }
        public Guid ItemSaleId { get; }

        public PositionToOrderCannotBeAddedException(Guid orderId, Guid itemSaleId) : base($"Position with item '{itemSaleId}' cannot be added to Order with id '{orderId}'.")
        {
            OrderId = orderId;
            ItemSaleId = itemSaleId;
        }
    }
}
