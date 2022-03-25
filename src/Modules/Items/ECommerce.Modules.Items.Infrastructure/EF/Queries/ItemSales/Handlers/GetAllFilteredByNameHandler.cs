using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.ItemSales;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.ItemSales.Handlers
{
    internal class GetAllFilteredByNameHandler : IQueryHandler<GetAllFilteredByName, IEnumerable<ItemSaleDto>>
    {
        private readonly DbSet<ItemSale> _itemsSale;

        public GetAllFilteredByNameHandler(ItemsDbContext itemsDbContext)
        {
            _itemsSale = itemsDbContext.ItemSales;
        }

        public async Task<IEnumerable<ItemSaleDto>> HandleAsync(GetAllFilteredByName query)
        {
            var itemsSale = await _itemsSale.Include(i => i.Item)
                                            .AsNoTracking()
                                            .Where(i => i.Active != null && i.Active.Value)
                                            .Where(i => i.Item.ItemName.ToLower().StartsWith(query.Name.ToLower()))
                                            .ToListAsync();
            return itemsSale?.Select(i => i.AsDto());
        }
    }
}