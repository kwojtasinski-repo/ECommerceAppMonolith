using ECommerce.Modules.Currencies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Repositories
{
    internal interface ICurrencyRepository
    {
        Task AddAsync(Currency currency);
        Task<Currency> GetAsync(Guid id);
        Task<Currency> GetDetailsAsync(Guid id);
        Task<IReadOnlyList<Currency>> GetAllAsync();
        Task UpdateAsync(Currency currency);
        Task DeleteAsync(Currency currency);
    }
}
