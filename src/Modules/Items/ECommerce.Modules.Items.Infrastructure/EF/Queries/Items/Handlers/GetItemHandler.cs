using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Items.Handlers
{
    internal class GetItemHandler : IQueryHandler<GetItem, ItemDetailsDto>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemDetailsDto> HandleAsync(GetItem query)
        {
            var item = await _itemRepository.GetAsync(query.ItemId);
            return item?.AsDetailsDto();
        }
    }
}
