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
        public decimal Cost => Price.Value;
        public decimal Rate => Currency.Rate;
        public string CurrencyCode => Currency.CurrencyCode;
        public Currency Currency { get; private set; }
        public Order? Order { get; private set; }
        public Guid UserId { get; private set; }

        private OrderItem() { }

        public OrderItem(Guid id, Guid itemId, ItemCart itemCart, decimal cost, string currencyCode, decimal rate, Guid userId)
        {
            ValidateItem(itemCart);
            Id = id;
            ItemCartId = itemId;
            ItemCart = itemCart;
            UserId = userId;
            Currency = new Currency(currencyCode, rate);
            Price = new Money(cost);
        }

        public static OrderItem Create(Guid id, ItemCart itemCart, decimal cost, string currencyCode, decimal rate, Guid userId)
        {
            var order = new OrderItem(id, itemCart.Id, itemCart, cost, currencyCode, rate, userId);
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
