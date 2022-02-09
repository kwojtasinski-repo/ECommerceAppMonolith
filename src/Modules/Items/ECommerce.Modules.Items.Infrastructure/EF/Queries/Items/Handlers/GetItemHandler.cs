using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Items.Handlers
{
    internal class GetItemHandler : IQueryHandler<GetItem, ItemDetailsDto>
    {
        private readonly DbSet<Item> _items;

        public GetItemHandler(ItemsDbContext itemsDbContext)
        {
            _items = itemsDbContext.Items;
        }

        public async Task<ItemDetailsDto> HandleAsync(GetItem query)
        {
            var item = await _items.Include(b => b.Brand)
                                             .Include(t => t.Type)
                                             .Include(i => i.ItemSale)
                                             .Where(i => i.Id == query.ItemId)
                                             .AsNoTracking()
                                             .SingleOrDefaultAsync();
            return item?.AsDetailsDto();
        }
    }
}