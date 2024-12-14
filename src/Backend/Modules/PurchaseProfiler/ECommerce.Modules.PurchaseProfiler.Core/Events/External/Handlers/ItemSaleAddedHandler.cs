using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleAddedHandler : IEventHandler<ItemSaleAdded>
    {
        private readonly ILogger<ItemSaleAddedHandler> _logger;
        private readonly IProductRepository _productRepository;

        public ItemSaleAddedHandler(ILogger<ItemSaleAddedHandler> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task HandleAsync(ItemSaleAdded @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                await _productRepository.AddAsync(new Entities.Product
                {
                    ProductId = @event.ItemId,
                    ProductSaleId = @event.Id,
                    Cost = @event.Cost,
                    IsActivated = true
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(ItemSaleAddedHandler));
            }
        }
    }
}
