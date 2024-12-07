using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class OrderRepository(SalesDbContext salesDbContext) : IOrderRepository
    {
        public async Task AddAsync(Order order)
        {
            await salesDbContext.Orders.AddAsync(order);
            await salesDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            salesDbContext.Orders.Remove(order);
            await salesDbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetAsync(Guid id)
        {
            return await salesDbContext.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Order?> GetDetailsAsync(Guid id)
        {
            return await salesDbContext.Orders.Where(o => o.Id == id)
                .Include(oi => oi.OrderItems)
                .Include(oi => oi.OrderItems).ThenInclude(i => i.ItemCart)
                .Include(p => p.Payments)
                .FirstOrDefaultAsync();
        }

        public Task<Order?> GetLatestOrderOnDateAsync(DateTime dateTime)
        {
            return salesDbContext.Orders.Where(o => o.CreateOrderDate.Date == dateTime.Date).OrderByDescending(o => o.CreateOrderDate).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            salesDbContext.Orders.Update(order);
            await salesDbContext.SaveChangesAsync();
        }
    }
}
