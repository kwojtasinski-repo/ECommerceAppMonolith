using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Policies.Items;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class DeleteItemHandler : ICommandHandler<DeleteItem>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemDeletionPolicy _itemDeletionPolicy;

        public DeleteItemHandler(IItemRepository itemRepository, IItemDeletionPolicy itemDeletionPolicy)
        {
            _itemRepository = itemRepository;
            _itemDeletionPolicy = itemDeletionPolicy;
        }

        public async Task HandleAsync(DeleteItem command)
        {
            var item = await _itemRepository.GetAsync(command.ItemId);

            if (item is null)
            {
                throw new ItemNotFoundException(command.ItemId);
            }

            if (await _itemDeletionPolicy.CanDeleteAsync(item) is false)
            {
                throw new CannotDeleteItemException(item.Id);
            }

            await _itemRepository.DeleteAsync(item);
        }
    }
}
