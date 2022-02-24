using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public OrderRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(Order order)
        {
            await _salesDbContext.Orders.AddAsync(order);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _salesDbContext.Orders.Remove(order);
            await _salesDbContext.SaveChangesAsync();
        }

        public Task<Order> GetAsync(Guid id)
        {
            var order = _salesDbContext.Orders.Where(o => o.Id == id).SingleOrDefaultAsync();
            return order;
        }

        public Task<Order> GetDetailsAsync(Guid id)
        {
            var order = _salesDbContext.Orders.Where(o => o.Id == id)
                .Include(oi => oi.OrderItems)
                .Include(oi => oi.OrderItems).ThenInclude(i => i.ItemCart)
                .Include(p => p.Payments)
                .SingleOrDefaultAsync();
            return order;
        }

        public Task<Order> GetLatestOrderOnDateAsync(DateTime dateTime)
        {
            var order = _salesDbContext.Orders.Where(o => o.CreateOrderDate.Date == dateTime.Date).OrderByDescending(o => o.CreateOrderDate).FirstOrDefaultAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _salesDbContext.Orders.Update(order);
            await _salesDbContext.SaveChangesAsync();
        }
    }
}
