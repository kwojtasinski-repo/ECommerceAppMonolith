using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Messagging;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers
{
    internal class ItemSaleActivateHandler : ICommandHandler<ItemSaleActivate>
    {
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ItemSaleActivateHandler(IItemSaleRepository itemSaleRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _itemSaleRepository = itemSaleRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ItemSaleActivate command)
        {
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            itemSale.Activate();
            await _itemSaleRepository.UpdateAsync(itemSale);

            var integrationEvents = _eventMapper.MapAll(itemSale.Events);
            await _messageBroker.PublishAsync(integrationEvents.ToArray());
        }
    }
}
