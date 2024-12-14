using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;
using ECommerce.Modules.Sales.Domain.Orders.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid ItemCartId { get; private set; }
        public ItemCart ItemCart { get; private set; }
        public Money Price { get; private set; }
        public Currency Currency { get; private set; }
        public Order? Order { get; private set; }
        public Guid UserId { get; private set; }

        private OrderItem() { }

        public OrderItem(Guid id, Guid itemId, ItemCart itemCart, decimal cost, string currencyCode, decimal rate, Guid userId, Order? order = null)
        {
            ValidateItem(itemCart);
            Id = id;
            ItemCartId = itemId;
            ItemCart = itemCart;
            UserId = userId;
            Currency = new Currency(currencyCode, rate);
            Price = new Money(cost);
            Order = order;
        }

        public static OrderItem Create(Guid id, ItemCart itemCart, decimal cost, string currencyCode, decimal rate, Guid userId)
        {
            var orderItem = new OrderItem(id, itemCart.Id, itemCart, cost, currencyCode, rate, userId);
            return orderItem;
        }
        
        public static OrderItem Create(Guid id, Order order, ItemCart itemCart, decimal cost, string currencyCode, decimal rate, Guid userId)
        {
            var orderItem = new OrderItem(id, itemCart.Id, itemCart, cost, currencyCode, rate, userId, order);
            return orderItem;
        }


        public static OrderItem Create(Guid id, ItemCart itemCart, decimal sourceCost, string sourceCurrencyCode, decimal sourceRate, string targetCurrencyCode, decimal targetRate, Guid userId)
        {
            var sourceCurrency = new Currency(sourceCurrencyCode, sourceRate);
            var targetCurrency = new Currency(targetCurrencyCode, targetRate);
            var cost = sourceCost * sourceCurrency.Rate / targetCurrency.Rate;
            var order = new OrderItem(id, itemCart.Id, itemCart, cost, targetCurrency.CurrencyCode, targetCurrency.Rate, userId);
            return order;
        }

        public void ChangeRate(decimal rate)
        {
            Currency = Currency.ChangeRate(rate);
        }

        public void ChangeCurrencyCode(string code)
        {
            Currency = Currency.ChangeCode(code);
        }

        public void ChangeCost(decimal cost)
        {
            Price = Price.ChangeValue(cost);
        }

        private static void ValidateItem(ItemCart itemCart)
        {
            if (itemCart is null)
            {
                throw new ItemCartCannotBeNullException();
            }
        }
    }
}
