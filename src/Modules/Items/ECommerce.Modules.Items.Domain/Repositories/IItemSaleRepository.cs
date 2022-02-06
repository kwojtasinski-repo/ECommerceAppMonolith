using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    public interface IItemSaleRepository
    {
        Task<IReadOnlyList<ItemSale>> GetAllAsync();
        Task<ItemSale> GetAsync(Guid id);
        Task AddAsync(ItemSale itemSale);
        Task UpdateAsync(ItemSale itemSale);
        Task DeleteAsync(ItemSale itemSale);
    }
}
