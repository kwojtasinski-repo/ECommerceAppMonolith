using ECommerce.Modules.Contacts.Core.DAL;
using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Contacts.Tests.Integration
{
    [Collection("integrationAddresses")]
    public class AddressControllerTests : IClassFixture<TestApplicationFactory<Program>>,
           IClassFixture<TestContactsDbContext>
    {
        [Fact]
        public async Task given_valid_id_should_return_address()
        {
            var addresses = await AddSampleData();
            var address = addresses[0];
            Authenticate(_userId);

            var response = (await _client.Request($"{Path}/{address.Id}").GetAsync());
            var addressFromDb = await response.GetJsonAsync<AddressDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            addressFromDb.ShouldNotBeNull();
            addressFromDb.Id.ShouldBe(address.Id);
            addressFromDb.CityName.ShouldBe(address.CityName);
            addressFromDb.CountryName.ShouldBe(address.CountryName);
        }

        [Fact]
        public async Task given_valid_address_dto_should_add()
        {
            var customer = new Customer { Id = Guid.NewGuid(), FirstName = "Filip", LastName = "Szary", PhoneNumber = "123456789", Company = false, Active = true };
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            var addressDto = new AddressDto { CountryName = "Poland", CityName = "Nowa Sol", BuildingNumber = "1", CustomerId = customer.Id, StreetName = "Szkolna", ZipCode = "67-100" };
            Authenticate(_userId);

            var response = await _client.Request($"{Path}").PostJsonAsync(addressDto);
            var id = response.GetIdFromHeaders<Guid>(Path);
            var addressFromDb = _dbContext.Addresses.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            addressFromDb.ShouldNotBeNull();
            addressFromDb.CityName.ShouldBe(addressDto.CityName);
            addressFromDb.CountryName.ShouldBe(addressDto.CountryName);
        }

        [Fact]
        public async Task given_valid_address_dto_should_update()
        {
            var addresses = await AddSampleData();
            var address = addresses[1];
            Authenticate(_userId);
            var addressDto = new AddressDto { Id = address.Id, BuildingNumber = "123", LocaleNumber = "54", CityName = "Zielona Gora", CountryName = address.CountryName, CustomerId = address.CustomerId, StreetName = address.StreetName, ZipCode = address.ZipCode };
            
            var response = await _client.Request($"{Path}").PutJsonAsync(addressDto);
            var addressUpdated = await (await _client.Request($"{Path}/{address.Id}").GetAsync()).GetJsonAsync<AddressDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            addressUpdated.ShouldNotBeNull();
            addressUpdated.BuildingNumber.ShouldNotBe(address.BuildingNumber);
            addressUpdated.LocaleNumber.ShouldNotBe(address.LocaleNumber);
            addressUpdated.CityName.ShouldNotBe(address.CityName);
            addressUpdated.CountryName.ShouldBe(address.CountryName);
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "currencies" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private const string Path = "contacts-module/addresses";
        private readonly IFlurlClient _client;
        private readonly ContactsDbContext _dbContext;
        private readonly Guid _userId;

        public AddressControllerTests(TestApplicationFactory<Program> factory, TestContactsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
            _userId = dbContext.UserId;
        }

        private async Task<List<Address>> AddSampleData()
        {
            var addresses = GetSampleData();
            var address1 = addresses[0];
            var address2 = addresses[1];
            await _dbContext.Addresses.AddAsync(address1);
            await _dbContext.Addresses.AddAsync(address2);
            await _dbContext.SaveChangesAsync();
            return addresses;
        }

        private List<Address> GetSampleData()
        {
            var address1 = new Address
            {
                Id = Guid.NewGuid(),
                ZipCode = "65-010",
                BuildingNumber = "1",
                CityName = "ZGora",
                CountryName = "POLAND",
                CustomerId = Guid.NewGuid(),
                StreetName = "Dluga",
                Active = true,
                Customer = new Customer { Id = Guid.NewGuid(), FirstName = "Johny", LastName = "Bravo", PhoneNumber = "123456789", UserId = _userId, Company = false, Active = true }
            };
            var address2 = new Address
            {
                Id = Guid.NewGuid(),
                ZipCode = "65-010",
                BuildingNumber = "5",
                LocaleNumber = "10",
                CityName = "ZGora",
                CountryName = "POLAND",
                CustomerId = Guid.NewGuid(),
                StreetName = "Wysoka",
                Active = true,
                Customer = new Customer { Id = Guid.NewGuid(), FirstName = "George", LastName = "Tanker", PhoneNumber = "123456789", UserId = _userId, Company = false, Active = true }
            };
            return new List<Address> { address1, address2 };
        }


    }
}