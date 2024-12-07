using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Events;
using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Modules.Sales.Application.Orders.Events.Handlers
{
    internal class PaymentAddedHandler : IEventHandler<PaymentAdded>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;

        public PaymentAddedHandler(IOrderRepository orderRepository, IClock clock, IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _clock = clock;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(PaymentAdded @event)
        {
            var order = await _orderRepository.GetDetailsAsync(@event.OrderId);

            if (order is null)
            {
                return;
            }

            order.MarkAsPaid();
            order.SetOrderApprovedDate(_clock.CurrentDate());
            await _orderRepository.UpdateAsync(order);
            await _messageBroker.PublishAsync(order.AsOrderPaid(@event.PaymentDate));
        }
    }
}
