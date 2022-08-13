using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Application.Payments.Exceptions;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Payments.Commands.Handlers
{
    internal class DeletePaymentHandler : ICommandHandler<DeletePayment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMessageBroker _messageBroker;

        public DeletePaymentHandler(IPaymentRepository paymentRepository, IMessageBroker messageBroker)
        {
            _paymentRepository = paymentRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeletePayment command)
        {
            var payment = await _paymentRepository.GetAsync(command.PaymentId);

            if (payment is null)
            {
                throw new PaymentNotFoundException(command.PaymentId);
            }

            await _paymentRepository.DeleteAsync(payment);
            await _messageBroker.PublishAsync(new PaymentDeleted(payment.Id, payment.Order.Id));
        }
    }
}
