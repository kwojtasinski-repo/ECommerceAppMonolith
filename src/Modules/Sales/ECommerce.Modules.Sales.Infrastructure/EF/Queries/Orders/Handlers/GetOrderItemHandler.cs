using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Modules.Sales.Infrastructure.EF.Mappings;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Queries.Orders.Handlers
{
    internal class GetOrderItemHandler : IQueryHandler<GetOrderItem, OrderItemDto>
    {
        private readonly SalesDbContext _dbContext;

        public GetOrderItemHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItemDto> HandleAsync(GetOrderItem query)
        {
            var orderItem = await _dbContext.OrderItems
                   .Include(ic => ic.ItemCart)
                   .Where(o => o.Id == query.OrderItemId)
                   .AsNoTracking()
                   .SingleOrDefaultAsync();
            return orderItem?.AsDto();
        }
    }
}
