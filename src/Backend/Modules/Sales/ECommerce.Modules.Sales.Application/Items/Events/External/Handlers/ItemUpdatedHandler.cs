using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class ItemUpdatedHandler : IEventHandler<ItemUpdated>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemUpdatedHandler> _logger;

        public ItemUpdatedHandler(IItemRepository itemRepository, ILogger<ItemUpdatedHandler> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ItemUpdated @event)
        {
            var item = await _itemRepository.GetAsync(@event.Id);

            if (item is null)
            {
                _logger.LogInformation($"Item with id: '{@event.Id}' not exists.");
                return;
            }

            item.BrandName = @event.BrandName;
            item.Description = @event.Description;
            item.ItemName = @event.ItemName;
            item.ImagesUrl = @event.ImagesUrl;
            item.Tags = @event.Tags;
            item.TypeName = @event.TypeName;

            await _itemRepository.UpdateAsync(item);
            _logger.LogInformation($"Updated an Item with id '{item.Id}'");
        }
    }
}
