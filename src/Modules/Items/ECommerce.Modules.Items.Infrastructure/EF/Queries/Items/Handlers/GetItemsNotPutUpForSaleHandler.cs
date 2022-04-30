using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Items.Handlers
{
    internal class GetItemsNotPutUpForSaleHandler : IQueryHandler<GetItemsNotPutUpForSale, IEnumerable<ItemDto>>
    {
        private readonly ItemsDbContext _dbContext;

        public GetItemsNotPutUpForSaleHandler(ItemsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ItemDto>> HandleAsync(GetItemsNotPutUpForSale query)
        {
            var items = await _dbContext.Items
                                        .Include(b => b.Brand)
                                        .Include(t => t.Type)
                                        .Include(it => it.ItemSale)
                                        .Where(i => i.ItemSale == null)
                                        .AsNoTracking()
                                        .ToListAsync();
            return items?.Select(i => i.AsDto());
        }
    }
}
