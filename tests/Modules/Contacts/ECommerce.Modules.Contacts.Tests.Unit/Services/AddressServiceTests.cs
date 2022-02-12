using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Modules.Contacts.Core.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Contacts.Tests.Unit.Services
{
    public class AddressServiceTests
    {
        [Fact]
        public async Task given_valid_address_should_add()
        {
            var address = new AddressDto
            {
                CountryName = "Poland",
                CityName = "Zielona Gora",
                CustomerId = Guid.NewGuid(),
                StreetName = "Wysoka",
                ZipCode = "65-010",
                BuildingNumber = "1a",
                LocaleNumber = "2"
            };

            await _service.AddAsync(address);

            await _addressRepository.Received(1).AddAsync(Arg.Any<Address>());
        }

        [Fact]
        public async Task given_valid_address_should_update()
        {
            var address = new AddressDto
            {
                Id = Guid.NewGuid(),
                CountryName = "Poland",
                CityName = "Zielona Gora",
                CustomerId = Guid.NewGuid(),
                StreetName = "Wysoka",
                ZipCode = "65-010",
                BuildingNumber = "2a",
                LocaleNumber = "2"
            };

            await _service.UpdateAsync(address);

            await _addressRepository.Received(1).UpdateAsync(Arg.Any<Address>());
        }

        private readonly AddressService _service;
        private readonly IAddressRepository _addressRepository;

        public AddressServiceTests()
        {
            _addressRepository = Substitute.For<IAddressRepository>();
            _service = new AddressService(_addressRepository);
        }
    }
}