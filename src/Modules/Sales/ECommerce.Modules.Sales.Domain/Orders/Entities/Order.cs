using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;
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
        public Money Price { get; private set; }
        public Currency Currency { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid UserId { get; private set; }
        public bool Paid { get; private set; }

        public IEnumerable<OrderItem> OrderItems => _orderItems;
        private ICollection<OrderItem> _orderItems = new List<OrderItem>();
        public IEnumerable<Payment>? Payments { get; private set; }

        private Order() { }

        public Order(AggregateId id, string orderNumber, decimal cost, string currencyCode, decimal rate, Guid customerId, Guid userId, DateTime createOrderDate, DateTime? orderApprovedDate = null, bool paid = false, ICollection<OrderItem> orderItems = null)
        {
            ValidateOrderNumber(orderNumber);
            ValidateCost(cost);
            Id = id;
            OrderNumber = orderNumber;
            Price = new Money(cost);
            Currency = new Currency(currencyCode, rate);
            Paid = paid;
            CustomerId = customerId;
            UserId = userId;
            CreateOrderDate = createOrderDate;
            OrderApprovedDate = orderApprovedDate;
            _orderItems = orderItems ?? new List<OrderItem>();
        }
        
        public Order(AggregateId id, string orderNumber, string currencyCode, decimal rate, Guid customerId, Guid userId, DateTime createOrderDate, DateTime? orderApprovedDate = null, bool paid = false, ICollection<OrderItem> orderItems = null)
        {
            ValidateOrderNumber(orderNumber);
            Id = id;
            OrderNumber = orderNumber;
            Price = Money.Zero;
            Currency = new Currency(currencyCode, rate);
            Paid = paid;
            CustomerId = customerId;
            UserId = userId;
            CreateOrderDate = createOrderDate;
            OrderApprovedDate = orderApprovedDate;
            _orderItems = orderItems ?? new List<OrderItem>();
        }   
        
        public Order(AggregateId id, string orderNumber, Guid customerId, Guid userId, DateTime createOrderDate, DateTime? orderApprovedDate = null, bool paid = false, ICollection<OrderItem> orderItems = null)
        {
            ValidateOrderNumber(orderNumber);
            Id = id;
            OrderNumber = orderNumber;
            Price = Money.Zero;
            Currency = Currency.Default();
            Paid = paid;
            CustomerId = customerId;
            UserId = userId;
            CreateOrderDate = createOrderDate;
            OrderApprovedDate = orderApprovedDate;
            _orderItems = orderItems ?? new List<OrderItem>();
        }

        public static Order Create(AggregateId id, string orderNumber, decimal cost, string currencyCode, decimal rate, Guid customerId, Guid userId, DateTime createOrderDate)
        {
            var order = new Order(id, orderNumber, cost, currencyCode, rate, customerId, userId, createOrderDate);
            return order;
        }
        
        public static Order Create(AggregateId id, string orderNumber, string currencyCode, decimal rate, Guid customerId, Guid userId, DateTime createOrderDate)
        {
            var order = new Order(id, orderNumber, currencyCode, rate, customerId, userId, createOrderDate);
            return order;
        }

        public static Order Create(AggregateId id, string orderNumber, string currencyCode, Guid customerId, Guid userId, DateTime createOrderDate)
        {
            var currency = Currency.Default();
            currency = currency.ChangeCode(currencyCode);
            var order = new Order(id, orderNumber, currency.CurrencyCode, currency.Rate, customerId, userId, createOrderDate);
            return order;
        }

        public static Order Create(AggregateId id, string orderNumber, Guid customerId, Guid userId, DateTime createOrderDate)
        {
            var order = new Order(id, orderNumber, customerId, userId, createOrderDate);
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

        internal void RefreshCost()
        {
            if (_orderItems is null)
            {
                throw new OrderItemsCannotBeNullException();
            }

            if (_orderItems.Any(ic => ic.ItemCart is null))
            {
                throw new CannotRefreshCostWhenItemCartIsNullException(Id);
            }

            var cost = decimal.Zero;

            foreach (var orderItem in _orderItems)
            {
                cost += orderItem.Price.Value;
            }

            Price = new Money(cost);
        }

        public void MarkAsPaid()
        {
            Paid = true;
        }

        public void MarkAsUnpaid()
        {
            Paid = false;
        }

        public void ChangeCurrency(string currencyCode, decimal rate)
        {
            Currency = new Currency(currencyCode, rate);
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
