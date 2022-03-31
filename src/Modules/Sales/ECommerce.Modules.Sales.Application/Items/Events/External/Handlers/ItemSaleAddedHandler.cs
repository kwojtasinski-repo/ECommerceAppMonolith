using ECommerce.Modules.Sales.Application.Items.Exceptions;
using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
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
    internal sealed class ItemSaleAddedHandler : IEventHandler<ItemSaleAdded>
    {
        private IItemSaleRepository _itemSaleRepository;
        private IItemRepository _itemRepository;
        private readonly ILogger<ItemSaleAddedHandler> _logger;

        public ItemSaleAddedHandler(IItemSaleRepository itemSaleRepository, IItemRepository itemRepository, ILogger<ItemSaleAddedHandler> logger)
        {
            _itemSaleRepository = itemSaleRepository;
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemSaleAdded @event)
        {
            var exists = await _itemSaleRepository.ExistsAsync(@event.Id);

            if (exists)
            {
                _logger.LogInformation($"ItemSale with id: '{@event.Id}' already exists.");
                return;
            }

            var item = await _itemRepository.GetAsync(@event.ItemId);
            
            if (item is null) 
            {
                _logger.LogInformation($"Item with id: '{@event.ItemId}' was not found.");
                return;
            }

            var itemSale = new ItemSale(@event.Id, item, @event.Cost, @event.CurrencyCode);
            await _itemSaleRepository.AddAsync(itemSale);
            _logger.LogInformation($"Added an ItemSale with id '{itemSale.Id}'");
        }
    }
}
