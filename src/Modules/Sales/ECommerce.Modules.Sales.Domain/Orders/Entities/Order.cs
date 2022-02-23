using ECommerce.Modules.Sales.Domain.Orders.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class Order : AggregateRoot
    {
        public string OrderNumber { get; private set; }
        public decimal Cost { get; private set; }
        public bool Paid { get; private set; }

        public IEnumerable<OrderItem> OrderItems => _orderItems;
        private ICollection<OrderItem> _orderItems;

        private Order() { }

        public Order(AggregateId id, string orderNumber, decimal cost, bool paid = false, ICollection<OrderItem> orderItems = null)
        {
            ValidateOrderNumber(orderNumber);
            ValidateCost(cost);
            Id = id;
            OrderNumber = orderNumber;
            Cost = cost;
            Paid = paid;
            _orderItems = orderItems;
        }

        public static Order Create(string paymentNumber, decimal cost)
        {
            var order = new Order(Guid.NewGuid(), paymentNumber, cost);
            return order;
        }

        public void AddOrderItems(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems is null)
            {
                throw new OrderItemsCannotBeNullException();
            }

            if (!orderItems.Any())
            {
                return;
            }

            foreach(var orderItem in orderItems)
            {
                _orderItems.Add(orderItem);
            }
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new OrderItemCannotBeNullException();
            }

            _orderItems.Add(orderItem);
        }

        public void DeleteOrderItem(OrderItem orderItem)
        {
            var orderItemToDelete = _orderItems.Where(oi => oi.Id == orderItem.Id).SingleOrDefault();

            if (orderItemToDelete is null)
            {
                throw new OrderItemNotFoundException(Id, orderItem.Id);
            }

            _orderItems.Remove(orderItemToDelete);
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
