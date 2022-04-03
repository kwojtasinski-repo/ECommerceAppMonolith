using ECommerce.Modules.Sales.Domain.Orders.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ItemCart CreateSampleItemCart(Guid id, decimal cost, string currencyCode)
        {
            var itemCart = new ItemCart(id, "Item #1", "Brand #1", "Type #1", "description",
                            null, null, cost, currencyCode, DateTime.UtcNow);
            return itemCart;
        }
    }
}
