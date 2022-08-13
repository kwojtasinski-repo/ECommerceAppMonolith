using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Application.Payments.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Time;
using System.Text;

namespace ECommerce.Modules.Sales.Application.Payments.Commands.Handlers
{
    internal class CreatePaymentHandler : ICommandHandler<CreatePayment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;
        private readonly IContext _context;

        public CreatePaymentHandler(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IClock clock, IMessageBroker messageBroker, IContext context)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _clock = clock;
            _messageBroker = messageBroker;
            _context = context;
        }

        public async Task HandleAsync(CreatePayment command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var currentDate = _clock.CurrentDate();
            var latestOrder = await _paymentRepository.GetLatestOrderOnDateAsync(currentDate);

            int number = 1;
            if (latestOrder is not null)
            {
                var lastOrderNumberToday = latestOrder.PaymentNumber;
                var stringNumber = lastOrderNumberToday.Substring(15);//16
                int.TryParse(stringNumber, out number);
                number++;
            }

            var paymentNumber = new StringBuilder("PAY/")
               .Append(currentDate.Year).Append('/').Append(currentDate.Month.ToString("d2"))
               .Append('/').Append(currentDate.Day.ToString("00")).Append('/').Append(number).ToString();

            var payment = Payment.Create(command.Id, paymentNumber, order, _context.Identity.Id, currentDate);
            await _paymentRepository.AddAsync(payment);
            await _messageBroker.PublishAsync(new PaymentAdded(payment.Id, command.OrderId));
        }
    }
}
