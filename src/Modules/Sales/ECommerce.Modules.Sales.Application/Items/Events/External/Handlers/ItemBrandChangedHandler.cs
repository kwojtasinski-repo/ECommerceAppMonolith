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
    internal class ItemBrandChangedHandler : IEventHandler<ItemBrandChanged>
    {
        private IItemRepository _itemRepository;
        private readonly ILogger<ItemAddedHandler> _logger;

        public ItemBrandChangedHandler(IItemRepository itemRepository, ILogger<ItemAddedHandler> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemBrandChanged @event)
        {
            var item = await _itemRepository.GetAsync(@event.Id);

            if (item is null)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' was not found.");
                return;
            }

            item.BrandName = @event.BrandName;
            await _itemRepository.UpdateAsync(item);
            _logger.LogInformation($"Changed brand '{@event.BrandName}' in Item with id '{item.Id}'");
        }
    }
}
