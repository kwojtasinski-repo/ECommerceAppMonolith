using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<IReadOnlyList<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
        Task<Item?> GetProductDataAsync(Guid id);
    }
}
