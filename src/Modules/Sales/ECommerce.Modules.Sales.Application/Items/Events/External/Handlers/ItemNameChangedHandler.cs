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
    internal class ItemNameChangedHandler : IEventHandler<ItemNameChanged>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemNameChanged> _logger;

        public ItemNameChangedHandler(IItemRepository itemRepository, ILogger<ItemNameChanged> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemNameChanged @event)
        {
            var item = await _itemRepository.GetAsync(@event.Id);

            if (item is null)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' was not found.");
                return;
            }

            item.ItemName = @event.ItemName;
            await _itemRepository.UpdateAsync(item);
            _logger.LogInformation($"Changed name '{@event.ItemName}' in Item with id '{item.Id}'");
        }
    }
}
