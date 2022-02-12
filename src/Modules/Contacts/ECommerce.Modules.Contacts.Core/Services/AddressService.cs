using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Services
{
    internal class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Task AddAsync(AddressDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AddressDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AddressDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AddressDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
