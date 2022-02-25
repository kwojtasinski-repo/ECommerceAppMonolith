using ECommerce.Modules.Sales.Application.Payments.DTO;
using ECommerce.Modules.Sales.Application.Payments.Queries;
using ECommerce.Modules.Sales.Infrastructure.EF.Mappings;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Queries.Payments.Handlers
{
    internal class GetPaymentsHandler : IQueryHandler<GetPayments, IEnumerable<PaymentDto>>
    {
        private readonly SalesDbContext _dbContext;

        public GetPaymentsHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PaymentDto>> HandleAsync(GetPayments query)
        {
            var payments = await _dbContext.Payments
                                .Where(p => p.Id == query.UserId)
                                .AsNoTracking()
                                .ToListAsync();
            return payments.Select(p => p.AsDto());
        }
    }
}
