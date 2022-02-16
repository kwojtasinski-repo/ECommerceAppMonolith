using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }
        public Guid? OrderId { get; private set; }

        public OrderItem(Guid id, Guid itemId, Item item, Guid? orderId = null)
        {
            ValidateItem(item);
            Id = id;
            ItemId = itemId;
            Item = item;
            OrderId = orderId;
        }

        public static OrderItem Create(Item item)
        {
            var order = new OrderItem(Guid.NewGuid(), item.Id, item);
            return order;
        }

        private static void ValidateItem(Item item)
        {
            if (item is null)
            {
                throw new ItemCannotBeNullException();
            }
        }
    }
}
