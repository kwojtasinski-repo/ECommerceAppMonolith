using ECommerce.Modules.Sales.Domain.Orders.Entities;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void should_create_order_item()
        {
            var sourceCurrencyCode = "EUR";
            var sourceRate = 3M;
            var targetCurrencyCode = "USD";
            var targetRate = 2M;
            var sourceCost = 200M;
            var expectedCost = 300M;
            var itemCart = CreateSampleItemCart(Guid.NewGuid(), sourceCost, sourceCurrencyCode);

            var orderItem = OrderItem.Create(Guid.NewGuid(), itemCart, sourceCost, sourceCurrencyCode,
                               sourceRate, targetCurrencyCode, targetRate, Guid.NewGuid());

            orderItem.ShouldNotBeNull();
            orderItem.Currency.CurrencyCode.ShouldBe(targetCurrencyCode);
            orderItem.Currency.Rate.ShouldBe(targetRate);
            orderItem.Price.Value.ShouldBe(expectedCost);
        }

        [Fact]
        public void given_invalid_currency_should_throw_an_exception()
        {
            var sourceCurrencyCode = "EUR";
            var sourceRate = -3M;
            var targetCurrencyCode = "USD";
            var targetRate = 2M;
            var sourceCost = 200M;
            var itemCart = CreateSampleItemCart(Guid.NewGuid(), sourceCost, sourceCurrencyCode);
            var expectedException = new InvalidOperationException("Invalid currency");

            var exception = Record.Exception(() => OrderItem.Create(Guid.NewGuid(), itemCart, sourceCost, sourceCurrencyCode,
                               sourceRate, targetCurrencyCode, targetRate, Guid.NewGuid()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        private ItemCart CreateSampleItemCart(Guid id, decimal cost, string currencyCode)
        {
            var itemCart = new ItemCart(id, "Item #1", "Brand #1", "Type #1", "description",
                            null, null, cost, currencyCode, DateTime.UtcNow, Guid.NewGuid());
            return itemCart;
        }
    }
}
