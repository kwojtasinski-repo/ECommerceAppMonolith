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
    internal class ItemImagesChangedHandler : IEventHandler<ItemImagesChanged>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemImagesChanged> _logger;

        public ItemImagesChangedHandler(IItemRepository itemRepository, ILogger<ItemImagesChanged> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemImagesChanged @event)
        {
            var item = await _itemRepository.GetAsync(@event.Id);

            if (item is null)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' was not found.");
                return;
            }

            item.ImagesUrl = @event.Images;
            await _itemRepository.UpdateAsync(item);
            _logger.LogInformation($"Changed images '{@event.Images}' in Item with id '{item.Id}'");
        }
    }
}
