using ECommerce.Modules.Contacts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Repositories
{
    internal interface IAddressRepository
    {
        Task AddAsync(Address address);
        Task<Address> GetAsync(Guid id);
        Task<IReadOnlyList<Address>> GetAllAsync();
        Task Update(Address address);
        Task Delete(Address address);
    }
}
