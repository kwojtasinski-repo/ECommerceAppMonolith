using ECommerce.Modules.Sales.Domain.Currency.Entities;
using ECommerce.Modules.Sales.Domain.Currency.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public CurrencyRateRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public Task AddAsync(CurrencyRate currencyRate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string currencyCode, DateOnly createdDate)
        {
            throw new NotImplementedException();
        }
    }
}
