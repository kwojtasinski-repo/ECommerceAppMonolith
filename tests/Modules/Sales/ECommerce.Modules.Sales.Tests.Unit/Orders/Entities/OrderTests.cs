using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Exceptions;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Entities
{
    public class OrderTests
    {
        [Fact]
        public void should_create_order()
        {
            var order = Order.Create(Guid.NewGuid(), "ORD", 1200M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

            order.ShouldNotBeNull();
        }

        [Fact]
        public void given_negative_cost_should_throw_an_exception()
        {
            var cost = -1200M;
            var expectedException = new OrderCostCannotBeNegativeException(cost);

            var exception = Record.Exception(() => Order.Create(Guid.NewGuid(), "ORD", cost, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderCostCannotBeNegativeException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public void given_invalid_order_number_should_throw_an_exception()
        {
            var expectedException = new OrderNumberCannotBeEmptyException();

            var exception = Record.Exception(() => Order.Create(Guid.NewGuid(), "", 1200M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderNumberCannotBeEmptyException>();
            exception.Message.ShouldBe(expectedException.Message);
        }
    }
}
