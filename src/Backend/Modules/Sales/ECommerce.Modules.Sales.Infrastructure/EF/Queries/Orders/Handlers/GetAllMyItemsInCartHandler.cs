using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Modules.Sales.Infrastructure.EF.Mappings;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Queries.Orders.Handlers
{
    internal class GetAllMyItemsInCartHandler : IQueryHandler<GetAllMyItemsInCart, IEnumerable<OrderItemDto>>
    {
        private readonly SalesDbContext _dbContext;

        public GetAllMyItemsInCartHandler(SalesDbContext salesDbContext)
        {
            _dbContext = salesDbContext;
        }

        public async Task<IEnumerable<OrderItemDto>> HandleAsync(GetAllMyItemsInCart query)
        {
            var orderItems = await _dbContext.OrderItems
                                        .Include(o => o.Order)
                                        .Include(ic => ic.ItemCart)
                                        .Where(oi => oi.UserId == query.UserId && oi.Order == null)
                                        .AsNoTracking()
                                        .ToListAsync();

            return orderItems.Select(oi => oi.AsDto());
        }
    }
}
