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
    internal class GetPaymentHandler : IQueryHandler<GetPayment, PaymentDto>
    {
        private readonly SalesDbContext _dbContext;

        public GetPaymentHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentDto> HandleAsync(GetPayment query)
        {
            var payment = await _dbContext.Payments
                                .Where(p => p.Id == query.PaymentId)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();
            return payment?.AsDto();
        }
    }
}
