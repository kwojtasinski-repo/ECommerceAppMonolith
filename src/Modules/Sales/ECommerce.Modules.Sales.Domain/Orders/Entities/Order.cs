using ECommerce.Modules.Sales.Domain.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class Order : AggregateRoot
    {
        public string OrderNumber { get; private set; }
        public DateTime CreateOrderDate { get; private set; }
        public DateTime? OrderApprovedDate { get; private set; }
        public decimal Cost { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid UserId { get; private set; }
        public bool Paid { get; private set; }

        public IEnumerable<OrderItem> OrderItems => _orderItems;
        private ICollection<OrderItem> _orderItems;
        public IEnumerable<Payment>? Payments { get; private set; }

        private Order() { }

        public Order(AggregateId id, string orderNumber, decimal cost, Guid customerId, Guid userId, DateTime createOrderDate, DateTime? orderApprovedDate = null, bool paid = false, ICollection<OrderItem> orderItems = null)
        {
            ValidateOrderNumber(orderNumber);
            ValidateCost(cost);
            Id = id;
            OrderNumber = orderNumber;
            Cost = cost;
            Paid = paid;
            CustomerId = customerId;
            UserId = userId;
            CreateOrderDate = createOrderDate;
            OrderApprovedDate = orderApprovedDate;
            _orderItems = orderItems;
        }

        public static Order Create(string orderNumber, decimal cost, Guid customerId, Guid userId, DateTime createOrderDate)
        {
            var order = new Order(Guid.NewGuid(), orderNumber, cost, customerId, userId, createOrderDate);
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
            ValidateOrderItem(orderItem);
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
