using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.ItemSales.Repositories
{
    public interface IItemRepository
    {
        Task<Item> GetAsync(Guid id);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task<bool> ExistsAsync(Guid id);
    }
}
