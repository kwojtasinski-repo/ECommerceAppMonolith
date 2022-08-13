using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.ItemSales;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.ItemSales.Handlers
{
    internal class GetItemsSaleHandler : IQueryHandler<GetItemsSale, IEnumerable<ItemSaleDto>>
    {
        private readonly DbSet<ItemSale> _itemsSale;

        public GetItemsSaleHandler(ItemsDbContext itemsDbContext)
        {
            _itemsSale = itemsDbContext.ItemSales;
        }

        public async Task<IEnumerable<ItemSaleDto>> HandleAsync(GetItemsSale query)
        {
            var itemsSale = await _itemsSale.Include(i => i.Item)
                                            .AsNoTracking()
                                            .Where(i => i.Active != null && i.Active.Value)
                                            .ToListAsync();
            return itemsSale?.Select(i => i.AsDto());
        }
    }
}
