using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    internal class ItemSaleNotFoundException : ECommerceException
    {
        public Guid ItemSaleId { get; }

        public ItemSaleNotFoundException(Guid itemSaleId) : base($"ItemSale with id '{itemSaleId}' was not found.")
        {
            ItemSaleId = itemSaleId;
        }
    }
}
