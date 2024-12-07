using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleAddedHandler : IEventHandler<ItemSaleAdded>
    {
        private readonly ILogger<ItemSaleAddedHandler> _logger;

        public ItemSaleAddedHandler(ILogger<ItemSaleAddedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemSaleAdded @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
