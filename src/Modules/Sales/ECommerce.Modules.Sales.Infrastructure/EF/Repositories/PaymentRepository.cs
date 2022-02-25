using ECommerce.Modules.Sales.Domain.Payments.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class PaymentRepository : IPaymentRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public PaymentRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(Payment payment)
        {
            await _salesDbContext.Payments.AddAsync(payment);
            await _salesDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment payment)
        {
            _salesDbContext.Payments.Remove(payment);
            await _salesDbContext.SaveChangesAsync();
        }

        public Task<Payment> GetAsync(Guid id)
        {
            var payment = _salesDbContext.Payments
                .Include(o => o.Order)
                .Where(i => i.Id == id).SingleOrDefaultAsync();
            return payment;
        }

        public Task<Payment> GetLatestOrderOnDateAsync(DateTime currentDate)
        {
            var payment = _salesDbContext.Payments
                .Where(p => p.PaymentDate.Date == currentDate.Date)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            _salesDbContext.Payments.Update(payment);
            await _salesDbContext.SaveChangesAsync();
        }
    }
}
