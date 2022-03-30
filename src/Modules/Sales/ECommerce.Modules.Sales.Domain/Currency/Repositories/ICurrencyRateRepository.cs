using ECommerce.Modules.Sales.Domain.Currency.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Currency.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task AddAsync(CurrencyRate currencyRate);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string currencyCode, DateOnly createdDate);
    }
}
