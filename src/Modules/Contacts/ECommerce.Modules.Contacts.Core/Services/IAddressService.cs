using ECommerce.Modules.Contacts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Services
{
    internal interface IAddressService
    {
        Task AddAsync(AddressDto dto);
        Task<AddressDto> GetAsync(Guid id);
        Task<IReadOnlyList<AddressDto>> GetAllAsync();
        Task UpdateAsync(AddressDto dto);
        Task DeleteAsync(Guid id);
    }
}
