using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.ItemSales.Repositories
{
    public interface IItemSaleRepository
    {
        Task<ItemSale> GetAsync(Guid id);
        Task AddAsync(ItemSale itemSale);
        Task UpdateAsync(ItemSale itemSale);
        Task<bool> ExistsAsync(Guid id);
    }
}
