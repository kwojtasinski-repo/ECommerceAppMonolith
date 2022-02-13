using ECommerce.Modules.Contacts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Repositories
{
    internal interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer> GetAsync(Guid id);
        Task<IReadOnlyList<Customer>> GetAllAsync();
        Task<IReadOnlyList<Customer>> GetAllByUserIdAsync(Guid userId);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }
}
