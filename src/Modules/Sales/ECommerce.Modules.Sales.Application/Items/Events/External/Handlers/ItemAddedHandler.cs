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
    internal sealed class ItemAddedHandler : IEventHandler<ItemAdded>
    {
        private IItemRepository _itemRepository;
        private readonly ILogger<ItemAddedHandler> _logger;

        public ItemAddedHandler(IItemRepository itemRepository, ILogger<ItemAddedHandler> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemAdded @event)
        {
            var exists = await _itemRepository.ExistsAsync(@event.Id);

            if (exists)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' already exists.");
                return;
            }

            var item = new Item(@event.Id, @event.ItemName, @event.BrandName, @event.TypeName, @event.Description,
                                @event.Tags, @event.ImagesUrl);
            await _itemRepository.AddAsync(item);
            _logger.LogInformation($"Added an item with id '{item.Id}'");
        }
    }
}
