using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.ItemsSale;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.ItemsSale.Handlers
{
    internal class GetItemSaleHandler : IQueryHandler<GetItemSale, ItemSaleDetailsDto>
    {
        private readonly DbSet<ItemSale> _itemsSale;

        public GetItemSaleHandler(ItemsDbContext itemsDbContext)
        {
            _itemsSale = itemsDbContext.ItemSales;
        }

        public async Task<ItemSaleDetailsDto> HandleAsync(GetItemSale query)
        {
            var itemSale = await _itemsSale.Include(i => i.Item)
                                           .Include(i => i.Item).ThenInclude(b => b.Brand)
                                           .Include(i => i.Item).ThenInclude(b => b.Type)
                                           .AsNoTracking()
                                           .Where(i => i.Id == query.ItemSaleId)
                                           .SingleOrDefaultAsync();
            return itemSale?.AsDetailsDto();
        }
    }
}
