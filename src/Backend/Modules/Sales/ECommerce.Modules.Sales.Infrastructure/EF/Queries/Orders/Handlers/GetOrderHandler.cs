using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Modules.Sales.Infrastructure.EF.Mappings;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Queries.Orders
{
    internal class GetOrderHandler : IQueryHandler<GetOrder, OrderDetailsDto>
    {
        private readonly SalesDbContext _dbContext;

        public GetOrderHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDetailsDto> HandleAsync(GetOrder query)
        {
            var order = await _dbContext.Orders
                .Include(oi => oi.OrderItems)
                .Include(oi => oi.OrderItems).ThenInclude(ic => ic.ItemCart)
                .Include(p => p.Payments)
                .Where(o => o.Id == query.OrderId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            return order?.AsDetailsDto();
        }
    }
}
