using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Core.Exceptions.Addresses;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Modules.Contacts.Core.Services;
using ECommerce.Modules.Contacts.Core.Validators;
using ECommerce.Shared.Abstractions.Exceptions;
using ECommerce.Shared.Abstractions.Validators;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task given_null_address_when_add_should_throw_an_exception()
        {
            var expectedException = new AddressCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _service.AddAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<AddressCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_address_when_add_should_throw_an_exception()
        {
            var expectedExceptions = new List<ECommerceException>
            {
                new CityNameCannotBeEmptyException(),
                new CountryNameCannotBeEmptyException(),
                new StreetNameCannotBeEmptyException(),
                new BuildingNumberCannotBeEmptyException(),
                new ZipCodeCannotBeEmptyException()
            };
            var expectedException = new ValidationException(expectedExceptions);

            var exception = await Record.ExceptionAsync(() => _service.AddAsync(new AddressDto()));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ValidationException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ValidationException)exception).Exceptions.SequenceEqual(expectedException.Exceptions);
        }

        [Fact]
        public async Task given_invalid_zip_code_when_add_should_throw_an_exception()
        {
            var address = new AddressDto
            {
                CountryName = "Poland",
                CityName = "Zielona Gora",
                CustomerId = Guid.NewGuid(),
                StreetName = "Wysoka",
                ZipCode = "65a010",
                BuildingNumber = "1a",
                LocaleNumber = "2"
            }; 
            var expectedExceptions = new List<ECommerceException>
            {
                new InvalidZipCodeFormatException(address.ZipCode),
            };
            var expectedException = new ValidationException(expectedExceptions);

            var exception = await Record.ExceptionAsync(() => _service.AddAsync(address));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ValidationException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ValidationException)exception).Exceptions.SequenceEqual(expectedException.Exceptions);
        }

        [Fact]
        public async Task given_valid_address_should_update()
        {
            var address = CreateAddress();
            var addressDto = new AddressDto
            {
                Id = address.Id,
                CountryName = "Poland",
                CityName = "Zielona Gora",
                CustomerId = Guid.NewGuid(),
                StreetName = "Wysoka",
                ZipCode = "65-010",
                BuildingNumber = "2a",
                LocaleNumber = "2"
            };
            _addressRepository.GetAsync(address.Id).Returns(address);

            await _service.UpdateAsync(addressDto);

            await _addressRepository.Received(1).UpdateAsync(Arg.Any<Address>());
        }

        private readonly AddressService _service;
        private readonly IAddressRepository _addressRepository;
        private readonly IValidator<AddressDto> _validator;

        public AddressServiceTests()
        {
            _validator = new AddressValidator();
            _addressRepository = Substitute.For<IAddressRepository>();
            _service = new AddressService(_addressRepository, _validator);
        }

        private Address CreateAddress()
        {
            return new Address
            {
                Id = Guid.NewGuid(),
                Active = true,
                ZipCode = "12-123",
                CityName = "Voive",
                BuildingNumber = "21",
                CountryName = "Germany",
                CustomerId = Guid.NewGuid(),
                StreetName = "Street",
            };
        }
    }
}