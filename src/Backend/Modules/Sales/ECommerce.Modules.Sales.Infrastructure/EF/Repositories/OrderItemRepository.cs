using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class OrderItemRepository : IOrderItemRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public OrderItemRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _salesDbContext.OrderItems.AddAsync(orderItem);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _salesDbContext.OrderItems.Remove(orderItem);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetAllByUserIdNotOrderedAsync(Guid userId)
        {
            var orderItems = await _salesDbContext.OrderItems
                                    .Include(o => o.Order)
                                    .Include(ic => ic.ItemCart)
                                    .Where(oi => oi.UserId == userId && oi.Order == null)
                                    .ToListAsync();
            return orderItems;
        }

        public Task<OrderItem> GetAsync(Guid id)
        {
            var orderItem = _salesDbContext.OrderItems.Where(oi => oi.Id == id)
                .Include(i => i.ItemCart).SingleOrDefaultAsync();
            return orderItem;
        }

        public Task<OrderItem> GetDetailsAsync(Guid id)
        {
            var orderItem = _salesDbContext.OrderItems.Where(oi => oi.Id == id)
                .Include(i => i.ItemCart)
                .Include(o => o.Order)
                .SingleOrDefaultAsync();
            return orderItem;
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _salesDbContext.OrderItems.Update(orderItem);
            await _salesDbContext.SaveChangesAsync();
        }
    }
}
