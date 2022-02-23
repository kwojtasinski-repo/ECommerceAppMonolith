using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal class ItemSaleCostChangedHandler : IEventHandler<ItemSaleCostChanged>
    {
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly ILogger<ItemSaleCostChangedHandler> _logger;

        public ItemSaleCostChangedHandler(IItemSaleRepository itemSaleRepository, ILogger<ItemSaleCostChangedHandler> logger)
        {
            _itemSaleRepository = itemSaleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemSaleCostChanged @event)
        {
            var itemSale = await _itemSaleRepository.GetAsync(@event.Id);

            if (itemSale is null)
            {
                _logger.LogInformation($"ItemSale with id: '{@event.Id}' was not found.");
                return;
            }

            itemSale.Cost = @event.Cost;
            await _itemSaleRepository.UpdateAsync(itemSale);
            _logger.LogInformation($"Updated '{@event.Cost}' cost in ItemSale with id '{itemSale.Id}'");
        }
    }
}
