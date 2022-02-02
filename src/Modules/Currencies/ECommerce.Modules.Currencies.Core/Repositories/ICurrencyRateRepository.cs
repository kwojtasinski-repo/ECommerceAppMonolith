using ECommerce.Modules.Currencies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Repositories
{
    internal interface ICurrencyRateRepository
    {
        Task AddAsync(CurrencyRate currencyRate);
        Task<CurrencyRate> GetAsync(Guid id);
        Task<IEnumerable<CurrencyRate>> GetAllAsync();
    }
}
