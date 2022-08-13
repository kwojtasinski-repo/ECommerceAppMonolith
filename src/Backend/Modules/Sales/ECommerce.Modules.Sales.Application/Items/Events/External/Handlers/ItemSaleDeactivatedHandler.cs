using ECommerce.Modules.Items.Application.Events;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class ItemSaleDeactivatedHandler : IEventHandler<ItemSaleDeactivated>
    {
        private IItemSaleRepository _itemSaleRepository;
        private ILogger<ItemSaleActivatedHandler> _logger;

        public ItemSaleDeactivatedHandler(IItemSaleRepository itemSaleRepository, ILogger<ItemSaleActivatedHandler> logger)
        {
            _itemSaleRepository = itemSaleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemSaleDeactivated @event)
        {
            var itemSale = await _itemSaleRepository.GetAsync(@event.Id);

            if (itemSale is null)
            {
                _logger.LogInformation($"ItemSale with id: '{@event.Id}' not exists.");
                return;
            }

            itemSale.Active = false;
            await _itemSaleRepository.UpdateAsync(itemSale);
            _logger.LogInformation($"Deactivated an ItemSale with id '{itemSale.Id}'");
        }
    }
}
