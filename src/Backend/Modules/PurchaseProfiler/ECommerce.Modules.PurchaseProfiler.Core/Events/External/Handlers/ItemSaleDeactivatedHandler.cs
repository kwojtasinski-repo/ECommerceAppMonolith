using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleDeactivatedHandler : IEventHandler<ItemSaleDeactivated>
    {
        private readonly ILogger<ItemSaleDeactivatedHandler> _logger;

        public ItemSaleDeactivatedHandler(ILogger<ItemSaleDeactivatedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemSaleDeactivated @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
