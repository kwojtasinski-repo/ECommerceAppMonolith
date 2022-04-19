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
    public class ChangeCustomerInOrderHandlerTests
    {
        [Fact]
        public async Task given_valid_customer_id_should_update_order()
        {
            var order = CreateOrder(Guid.NewGuid());
            var customerId = Guid.NewGuid();
            var command = new ChangeCustomerInOrder(order.Id, customerId);
            _orderRepository.GetDetailsAsync(order.Id).Returns(order);

            await _handler.HandleAsync(command);

            await _orderRepository.Received(1).UpdateAsync(order);
        }

        [Fact]
        public async Task given_invalid_order_should_throw_an_exception()
        {
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new ChangeCustomerInOrder(orderId, customerId);
            var expectedException = new OrderNotFoundException(orderId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_customer_id_and_paid_order_should_throw_an_exception()
        {
            var order = CreateOrder(Guid.NewGuid(), paid: true);
            var customerId = Guid.NewGuid();
            var command = new ChangeCustomerInOrder(order.Id, customerId);
            var expectedException = new CustomerCannotBeChangedInOrderException(order.Id);
            _orderRepository.GetDetailsAsync(order.Id).Returns(order);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        private Order CreateOrder(Guid id, bool paid = false)
        {
            var order = new Order(id, "Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, paid: paid);
            return order;
        }

        private readonly ChangeCustomerInOrderHandler _handler;
        private readonly IOrderRepository _orderRepository;

        public ChangeCustomerInOrderHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _handler = new ChangeCustomerInOrderHandler(_orderRepository);
        }
    }
}
