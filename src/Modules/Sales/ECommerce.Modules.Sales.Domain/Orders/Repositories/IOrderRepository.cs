using ECommerce.Modules.Sales.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetAsync(Guid id);
        Task UpdateAsync(Order order);
        Task AddAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
