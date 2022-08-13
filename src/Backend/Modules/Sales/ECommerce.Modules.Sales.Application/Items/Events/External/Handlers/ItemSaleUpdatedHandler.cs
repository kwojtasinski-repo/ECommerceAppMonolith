using ECommerce.Modules.Items.Application.Commands.ItemSales;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class ItemSaleUpdatedHandler : IEventHandler<ItemSaleUpdated>
    {
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly ILogger<ItemSaleUpdatedHandler> _logger;

        public ItemSaleUpdatedHandler(IItemSaleRepository itemSaleRepository, ILogger<ItemSaleUpdatedHandler> logger)
        {
            _itemSaleRepository = itemSaleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemSaleUpdated @event)
        {
            var itemSale = await _itemSaleRepository.GetAsync(@event.ItemSaleId);

            if (itemSale is null)
            {
                _logger.LogInformation($"ItemSale with id: '{@event.ItemSaleId}' not exists.");
                return;
            }

            itemSale.Cost = @event.ItemCost;
            itemSale.CurrencyCode = @event.CurrencyCode;
            
            await _itemSaleRepository.UpdateAsync(itemSale);
            _logger.LogInformation($"Updated an ItemSale with id '{itemSale.Id}'");
        }
    }
}
