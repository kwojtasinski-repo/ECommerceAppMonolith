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
    internal class ItemDescriptionChangedHandler : IEventHandler<ItemDescriptionChanged>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemDescriptionChangedHandler> _logger;

        public ItemDescriptionChangedHandler(IItemRepository itemRepository, ILogger<ItemDescriptionChangedHandler> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemDescriptionChanged @event)
        {
            var item = await _itemRepository.GetAsync(@event.Id);

            if (item is null)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' was not found.");
                return;
            }

            item.Description = @event.Description;
            await _itemRepository.UpdateAsync(item);
            _logger.LogInformation($"Changed description '{@event.Description}' in Item with id '{item.Id}'");
        }
    }
}
