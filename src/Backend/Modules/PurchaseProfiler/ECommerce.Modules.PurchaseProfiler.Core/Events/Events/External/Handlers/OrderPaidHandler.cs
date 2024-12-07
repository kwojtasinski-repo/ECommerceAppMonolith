using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.Events.External.Handlers
{
    internal class OrderPaidHandler : IEventHandler<OrderPaid>
    {
        private readonly ILogger<OrderPaidHandler> _logger;

        public OrderPaidHandler(ILogger<OrderPaidHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(OrderPaid @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
