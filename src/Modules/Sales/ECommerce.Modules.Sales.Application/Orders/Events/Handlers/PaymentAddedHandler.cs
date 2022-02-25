using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Orders.Events.Handlers
{
    internal class PaymentAddedHandler : IEventHandler<PaymentAdded>
    {
        private readonly IOrderRepository _orderRepository;

        public PaymentAddedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(PaymentAdded @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);

            if (order is null)
            {
                return;
            }

            order.MarkAsPaid();
            await _orderRepository.UpdateAsync(order);
        }
    }
}
