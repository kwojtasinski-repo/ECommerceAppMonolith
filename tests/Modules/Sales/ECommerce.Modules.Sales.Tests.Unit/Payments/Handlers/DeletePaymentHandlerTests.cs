using ECommerce.Modules.Sales.Application.Payments.Commands;
using ECommerce.Modules.Sales.Application.Payments.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Application.Payments.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using ECommerce.Shared.Abstractions.Messagging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Payments.Handlers
{
    public class DeletePaymentHandlerTests
    {
        [Fact]
        public async Task given_invalid_id_should_throw_an_exception()
        {
            var command = new DeletePayment(Guid.NewGuid());
            var expectedException = new PaymentNotFoundException(command.PaymentId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<PaymentNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_id_should_delete_payment()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 1200M, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            var payment = new Payment(Guid.NewGuid(), "PAY", order, order.UserId, DateTime.Now);
            var command = new DeletePayment(payment.Id);
            _paymentRepository.GetAsync(payment.Id).Returns(payment);

            await _handler.HandleAsync(command);

            await _paymentRepository.Received(1).DeleteAsync(payment);
        }

        [Fact]
        public async Task given_valid_id_when_delete_should_publish_event()
        {
            var order = new Order(Guid.NewGuid(), "ORD", 1200M, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            var payment = new Payment(Guid.NewGuid(), "PAY", order, order.UserId, DateTime.Now);
            var command = new DeletePayment(payment.Id);
            _paymentRepository.GetAsync(payment.Id).Returns(payment);

            await _handler.HandleAsync(command);

            var @event = new PaymentDeleted(payment.Id, payment.Order.Id);
            await _messageBroker.Received(1).PublishAsync(new IMessage[] { @event });
        }

        private readonly IPaymentRepository _paymentRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly DeletePaymentHandler _handler;

        public DeletePaymentHandlerTests()
        {
            _paymentRepository = Substitute.For<IPaymentRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _handler = new DeletePaymentHandler(_paymentRepository, _messageBroker);
        }
    }
}
