using ECommerce.Modules.Sales.Application.Payments.Events;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Orders.Events.Handlers
{
    internal class PaymentRefundedHandler : IEventHandler<PaymentRefunded>
    {
        private readonly IOrderRepository _orderRepository;

        public PaymentRefundedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(PaymentRefunded @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);

            if (order is null)
            {
                return;
            }

            order.MarkAsUnpaid();
            await _orderRepository.UpdateAsync(order);
        }
    }
}
