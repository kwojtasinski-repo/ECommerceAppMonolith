using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class ItemSaleActivatedHandler : IEventHandler<ItemSaleActivated>
    {
        private readonly ILogger<ItemSaleActivatedHandler> _logger;
        private readonly IProductRepository _productRepository;

        public ItemSaleActivatedHandler(ILogger<ItemSaleActivatedHandler> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task HandleAsync(ItemSaleActivated @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                var product = await _productRepository.GetByProductSaleIdAsync(@event.Id);
                if (product is null)
                {
                    _logger.LogWarning("Product with itemSaleId '{itemSale}' was not found", @event.Id);
                    return;
                }

                product.IsActivated = true;
                await _productRepository.UpdateAsync(product);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(ItemSaleActivatedHandler));
            }
        }
    }
}
