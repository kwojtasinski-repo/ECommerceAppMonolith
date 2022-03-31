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

        [Fact]
        public void given_valid_order_item_should_add_to_order()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 120M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            var itemCart = new ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description", null, null, 120M, "PLN");
            var orderItem = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), itemCart, Guid.NewGuid());

            order.AddOrderItem(orderItem);

            order.OrderItems.Count().ShouldBe(1);
        }

        [Fact]
        public void given_null_order_item_should_throw_an_exception()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 120M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            var expectedException = new OrderItemCannotBeNullException();

            var exception = Record.Exception(() => order.AddOrderItem(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderItemCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public void given_null_order_items_should_throw_an_exception()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 120M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            var expectedException = new OrderItemsCannotBeNullException();

            var exception = Record.Exception(() => order.AddOrderItems(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderItemsCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public void given_invalid_order_item_when_delete_from_order_should_throw_an_exception()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 120M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            var itemCart = new ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description", null, null, 120M, "PLN");
            var orderItem = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), itemCart, Guid.NewGuid());
            var expectedException = new OrderItemNotFoundException(order.Id, orderItem.Id);

            var exception = Record.Exception(() => order.DeleteOrderItem(orderItem));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderItemNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((OrderItemNotFoundException)exception).OrderId.ShouldBe(expectedException.OrderId);
            ((OrderItemNotFoundException)exception).OrderItemId.ShouldBe(expectedException.OrderItemId);
        }
    }
}
