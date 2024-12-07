using ECommerce.Modules.Items.Application.Events;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class ItemSaleActivatedHandler : IEventHandler<ItemSaleActivated>
    {
        private IItemSaleRepository _itemSaleRepository;
        private ILogger<ItemSaleActivatedHandler> _logger;

        public ItemSaleActivatedHandler(IItemSaleRepository itemSaleRepository, ILogger<ItemSaleActivatedHandler> logger)
        {
            _itemSaleRepository = itemSaleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemSaleActivated @event)
        {
            var itemSale = await _itemSaleRepository.GetAsync(@event.Id);
            
            if (itemSale is null)
            {
                _logger.LogInformation($"ItemSale with id: '{@event.Id}' not exists.");
                return;
            }

            itemSale.Active = true;
            await _itemSaleRepository.UpdateAsync(itemSale);
            _logger.LogInformation($"Activated an ItemSale with id '{itemSale.Id}'");
        }
    }
}
