using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class DeleteOrderItemHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_delete()
        {
            var orderItemId = Guid.NewGuid();
            var orderItem = CreateSampleOrderItem(orderItemId);
            _orderItemRepository.GetDetailsAsync(orderItemId).Returns(orderItem);
            var command = new DeleteOrderItem(orderItemId);

            await _handler.HandleAsync(command);

            await _orderItemRepository.Received(1).DeleteAsync(orderItem);
        }

        [Fact]
        public async Task given_invalid_order_item_id_should_throw_an_exception()
        {
            var orderItemId = Guid.NewGuid(); 
            var command = new DeleteOrderItem(orderItemId);
            var expectedException = new OrderItemNotFoundException(orderItemId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            ((OrderItemNotFoundException)exception).OrderItemId.ShouldBe(orderItemId);
        }

        [Fact]
        public async Task given_order_item_with_order_when_delete_should_throw_an_exception()
        {
            var orderItemId = Guid.NewGuid();
            var order = new Order(Guid.NewGuid(), "ORD", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
            var orderItem = CreateSampleOrderItem(orderItemId, order);
            _orderItemRepository.GetDetailsAsync(orderItemId).Returns(orderItem);
            var command = new DeleteOrderItem(orderItemId);
            var expectedException = new OrderItemCannotBeDeletedException(orderItemId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            ((OrderItemCannotBeDeletedException)exception).OrderItemId.ShouldBe(orderItemId);
        }

        private OrderItem CreateSampleOrderItem(Guid id, Order order = null)
        {
            var itemCart = CreateSampleItemCart(Guid.NewGuid());
            var orderItem = new OrderItem(id, itemCart.Id, itemCart, 1000M, "PLN", 1M, Guid.NewGuid(), order);
            return orderItem;
        }

        private ItemCart CreateSampleItemCart(Guid id)
        {
            var itemCart = new ItemCart(id, "Item #1", "Brand #1", "Type #1", "description", null,
                                    null, 1000M, "PLN", DateTime.UtcNow, Guid.NewGuid());
            return itemCart;
        }

        private readonly DeleteOrderItemHandler _handler;
        private readonly IOrderItemRepository _orderItemRepository;

        public DeleteOrderItemHandlerTests()
        {
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _handler = new DeleteOrderItemHandler(_orderItemRepository);
        }
    }
}
