using ECommerce.Modules.Contacts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Services
{
    internal interface ICustomerService
    {
        Task AddAsync(CustomerDto dto);
        Task<CustomerDetailsDto> GetAsync(Guid id);
        Task<IReadOnlyList<CustomerDto>> GetAllAsync();
        Task UpdateAsync(CustomerDto dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<CustomerDto>> GetAllByUserAsync(Guid userId);
    }
}
