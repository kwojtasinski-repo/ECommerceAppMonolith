using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleActivatedHandler : IEventHandler<ItemSaleActivated>
    {
        private readonly ILogger<ItemSaleActivatedHandler> _logger;

        public ItemSaleActivatedHandler(ILogger<ItemSaleActivatedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemSaleActivated @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            return Task.CompletedTask;
        }
    }
}
