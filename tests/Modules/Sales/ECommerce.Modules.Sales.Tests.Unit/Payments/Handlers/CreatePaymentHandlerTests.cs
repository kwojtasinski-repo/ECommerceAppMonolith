using ECommerce.Modules.Sales.Application.Payments.Commands;
using ECommerce.Modules.Sales.Application.Payments.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Application.Payments.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Payments.Handlers
{
    public class CreatePaymentHandlerTests
    {
        [Fact]
        public async Task given_invalid_id_should_throw_an_exception()
        {
            var command = new CreatePayment(Guid.NewGuid());
            var expectedException = new OrderNotFoundException(command.OrderId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<OrderNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_id_should_add_payment()
        {
            var currency = Currency.Default();
            var order = new Order(Guid.NewGuid(), "ORD", 1200M, currency.CurrencyCode, currency.Rate, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            var command = new CreatePayment(order.Id);
            _orderRepository.GetAsync(order.Id).Returns(order);

            await _handler.HandleAsync(command);

            await _paymentRepository.Received(1).AddAsync(Arg.Any<Payment>());
        }

        [Fact]
        public async Task given_valid_id_should_publish_event_when_add_payment()
        {
            var currency = Currency.Default();
            var order = new Order(Guid.NewGuid(), "ORD", 1200M, currency.CurrencyCode, currency.Rate, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            var command = new CreatePayment(order.Id);
            _orderRepository.GetAsync(order.Id).Returns(order);

            await _handler.HandleAsync(command);

            var @event = new PaymentAdded(command.Id, command.OrderId);
            await _messageBroker.Received(1).PublishAsync(new IMessage[] { @event });
        }

        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;
        private readonly IContext _context;
        private readonly CreatePaymentHandler _handler;

        public CreatePaymentHandlerTests()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _messageBroker = Substitute.For<IMessageBroker>();
            _context = Substitute.For<IContext>();
            _handler = new CreatePaymentHandler(_paymentRepository, _orderRepository, _clock, _messageBroker, _context);
        }
    }
}
