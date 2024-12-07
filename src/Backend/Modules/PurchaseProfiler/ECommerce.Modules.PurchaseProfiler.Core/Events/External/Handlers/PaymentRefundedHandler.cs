using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class PaymentRefundedHandler : IEventHandler<PaymentRefunded>
    {
        private readonly ILogger<PaymentRefundedHandler> _logger;

        public PaymentRefundedHandler(ILogger<PaymentRefundedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(PaymentRefunded @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
