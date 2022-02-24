using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Modules.Sales.Infrastructure.EF.Mappings;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Queries.Orders.Handlers
{
    internal class GetOrdersByUserIdHandler : IQueryHandler<GetOrdersByUserId, IEnumerable<OrderDto>>
    {
        private readonly SalesDbContext _dbContext;

        public GetOrdersByUserIdHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderDto>> HandleAsync(GetOrdersByUserId query)
        {
            var orders = await _dbContext.Orders.Where(o => o.UserId == query.UserId).ToListAsync();
            return orders.Select(o => o.AsDto());
        }
    }
}
