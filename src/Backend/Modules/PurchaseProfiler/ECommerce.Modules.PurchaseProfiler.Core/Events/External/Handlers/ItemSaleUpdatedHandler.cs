using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleUpdatedHandler : IEventHandler<ItemSaleUpdated>
    {
        private readonly ILogger<ItemSaleUpdatedHandler> _logger;
        private readonly IProductRepository _productRepository;

        public ItemSaleUpdatedHandler(ILogger<ItemSaleUpdatedHandler> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task HandleAsync(ItemSaleUpdated @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            var product = await _productRepository.GetByProductSaleIdAsync(@event.ItemSaleId);
            if (product is null)
            {
                _logger.LogWarning("Product with itemSaleId '{itemSale}' was not found", @event.ItemSaleId);
                return;
            }

            product.Cost = @event.ItemCost;
            await _productRepository.UpdateAsync(product);
        }
    }
}
