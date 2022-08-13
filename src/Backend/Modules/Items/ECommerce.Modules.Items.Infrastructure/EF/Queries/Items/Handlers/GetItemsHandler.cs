using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Items.Handlers
{
    internal class GetItemsHandler : IQueryHandler<GetItems, IEnumerable<ItemDto>>
    {
        private readonly DbSet<Item> _items;

        public GetItemsHandler(ItemsDbContext itemsDbContext)
        {
            _items = itemsDbContext.Items;
        }

        public async Task<IEnumerable<ItemDto>> HandleAsync(GetItems query)
        {
            var items = await _items.Include(b => b.Brand)
                                    .Include(t => t.Type)
                                    .AsNoTracking()
                                    .ToListAsync();
            return items?.Select(i => i.AsDto());
        }
    }
}