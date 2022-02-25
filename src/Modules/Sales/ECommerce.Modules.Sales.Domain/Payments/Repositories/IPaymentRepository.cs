using ECommerce.Modules.Sales.Domain.Payments.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Payments.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetAsync(Guid id);
        Task UpdateAsync(Payment payment);
        Task AddAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task<Payment> GetLatestOrderOnDateAsync(DateTime currentDate);
    }
}
