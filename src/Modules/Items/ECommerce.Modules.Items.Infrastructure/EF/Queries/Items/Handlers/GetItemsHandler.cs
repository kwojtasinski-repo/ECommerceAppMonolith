using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Items.Handlers
{
    internal class GetItemsHandler : IQueryHandler<GetItems, IEnumerable<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemsHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemDto>> HandleAsync(GetItems query)
        {
            var items = await _itemRepository.GetAllAsync();
            return items?.Select(i => i.AsDto());
        }
    }
}
