using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class PaymentRefundedHandler : IEventHandler<PaymentRefunded>
    {
        private readonly ILogger<PaymentRefundedHandler> _logger;
        private readonly IOrderRepository _orderRepository;

        public PaymentRefundedHandler(ILogger<PaymentRefundedHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(PaymentRefunded @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                var order = await _orderRepository.GetByOrderIdAsync(@event.OrderId);
                if (order is null)
                {
                    _logger.LogWarning("Order with orderId '{orderId}' was not found", @event.OrderId);
                    return;
                }

                await _orderRepository.DeleteAsync(order.Key!);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(OrderPaidHandler));
            }
        }
    }
}
