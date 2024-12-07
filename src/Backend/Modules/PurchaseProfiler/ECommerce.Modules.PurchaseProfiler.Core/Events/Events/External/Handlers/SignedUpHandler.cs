using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.Events.External.Handlers
{
    internal sealed class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly ILogger<SignedUpHandler> _logger;

        public SignedUpHandler(ILogger<SignedUpHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(SignedUp @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
