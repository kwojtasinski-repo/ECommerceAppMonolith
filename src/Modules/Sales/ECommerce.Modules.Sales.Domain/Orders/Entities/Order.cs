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
    public class Order : AggregateRoot
    {
        public string OrderNumber { get; private set; }
        public OrderItem OrderItem { get; private set; }
        public decimal Cost { get; private set; }
        public bool Paid { get; set; }

        public Order(AggregateId id, string orderNumber, OrderItem orderItem, decimal cost, bool paid = false)
        {
            ValidateOrderNumber(orderNumber);
            ValidateOrderItem(orderItem);
            ValidateCost(cost);
            Id = id;
            OrderNumber = orderNumber;
            OrderItem = orderItem;
            Cost = cost;
            Paid = paid;
        }

        public static Order Create(string paymentNumber, OrderItem orderItem, decimal cost)
        {
            var order = new Order(Guid.NewGuid(), paymentNumber, orderItem, cost);
            return order;
        }

        private static void ValidateOrderNumber(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new OrderNumberCannotBeEmptyException();
            }
        }

        private static void ValidateOrderItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new OrderItemCannotBeNullException();
            }
        }

        private static void ValidateCost(decimal cost)
        {
            if (cost < decimal.Zero)
            {
                throw new OrderCostCannotBeNegativeException(cost);
            }
        }
    }
}
