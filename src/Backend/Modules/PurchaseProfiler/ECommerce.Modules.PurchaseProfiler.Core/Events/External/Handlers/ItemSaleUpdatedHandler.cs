using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleUpdatedHandler : IEventHandler<ItemSaleUpdated>
    {
        private readonly ILogger<ItemSaleUpdatedHandler> _logger;

        public ItemSaleUpdatedHandler(ILogger<ItemSaleUpdatedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemSaleUpdated @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
