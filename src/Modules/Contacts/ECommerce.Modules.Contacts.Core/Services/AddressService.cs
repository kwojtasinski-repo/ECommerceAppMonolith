using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Exceptions.Addresses;
using ECommerce.Modules.Contacts.Core.Mappings;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Shared.Abstractions.Validators;

namespace ECommerce.Modules.Contacts.Core.Services
{
    internal class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IValidator<AddressDto> _validator;

        public AddressService(IAddressRepository addressRepository, IValidator<AddressDto> validator)
        {
            _addressRepository = addressRepository;
            _validator = validator;
        }

        public async Task AddAsync(AddressDto dto)
        {
            _validator.Validate(dto);
            dto.Id = Guid.NewGuid();
            await _addressRepository.AddAsync(dto.AsEntity());
        }

        public async Task<AddressDto> GetAsync(Guid id)
        {
            var address = await _addressRepository.GetAsync(id);
            return address.AsDto();
        }

        public async Task UpdateAsync(AddressDto dto)
        {
            _validator.Validate(dto);
            var address = await _addressRepository.GetAsync(dto.Id);

            if (address is null)
            {
                throw new AddressNotFoundException(dto.Id);
            }

            address.CountryName = dto.CountryName;
            address.CityName = dto.CityName;
            address.StreetName = dto.StreetName;
            address.ZipCode = dto.ZipCode;
            address.BuildingNumber = dto.BuildingNumber;
            address.LocaleNumber = dto.LocaleNumber;
            await _addressRepository.UpdateAsync(dto.AsEntity());
        }
    }
}
