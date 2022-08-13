using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Events;
using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Modules.Sales.Application.Orders.Events.Handlers
{
    internal class PaymentAddedHandler : IEventHandler<PaymentAdded>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClock _clock;

        public PaymentAddedHandler(IOrderRepository orderRepository, IClock clock)
        {
            _orderRepository = orderRepository;
            _clock = clock;
        }

        public async Task HandleAsync(PaymentAdded @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);

            if (order is null)
            {
                return;
            }

            order.MarkAsPaid();
            order.SetOrderApprovedDate(_clock.CurrentDate());
            await _orderRepository.UpdateAsync(order);
        }
    }
}
