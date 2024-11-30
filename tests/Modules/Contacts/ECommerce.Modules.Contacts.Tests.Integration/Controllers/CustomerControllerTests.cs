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

namespace ECommerce.Modules.Contacts.Tests.Integration.Controllers
{

    public class CustomerControllerTests : BaseTest, IAsyncLifetime
    {
        [Fact]
        public async Task given_valid_user_id_should_return_address()
        {
            Authenticate(_userId, client);

            var response = (await client.Request($"{Path}/me").GetAsync());
            var addressesFromDb = await response.GetJsonAsync<IEnumerable<CustomerDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            addressesFromDb.ShouldNotBeNull();
            addressesFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_address()
        {
            var customer = _customers.FirstOrDefault();
            Authenticate(_userId, client);

            var response = (await client.Request($"{Path}/{customer.Id}").GetAsync());
            var customerFromDb = await response.GetJsonAsync<CustomerDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            customerFromDb.ShouldNotBeNull();
            customerFromDb.LastName.ShouldBe(customer.LastName);
            customerFromDb.PhoneNumber.ShouldBe(customer.PhoneNumber);
        }

        [Fact]
        public async Task given_valid_dto_should_add()
        {
            var dto = new CustomerDto { FirstName = "Michael", LastName = "Employer", PhoneNumber = "123456789", Company = false };
            Authenticate(_userId, client);

            var response = await client.Request($"{Path}").PostJsonAsync(dto);
            var id = response.GetIdFromHeaders<Guid>(Path);
            var customerFromDb = dbContext.Customers.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            customerFromDb.ShouldNotBeNull();
            customerFromDb.FirstName.ShouldBe(dto.FirstName);
            customerFromDb.LastName.ShouldBe(dto.LastName);
            customerFromDb.PhoneNumber.ShouldBe(dto.PhoneNumber);
            customerFromDb.Company.ShouldBe(dto.Company);
        }

        [Fact]
        public async Task given_valid_dto_should_update()
        {
            var customer = _customers[1];
            Authenticate(_userId, client);
            var dto = new CustomerDto { Id = customer.Id, FirstName = "Michael", LastName = "Employer", PhoneNumber = "123456789", Company = false };

            var response = await client.Request($"{Path}/{customer.Id}").PutJsonAsync(dto);
            var customerUpdated = await (await client.Request($"{Path}/{customer.Id}").GetAsync()).GetJsonAsync<CustomerDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            customerUpdated.FirstName.ShouldNotBe(customer.FirstName);
            customerUpdated.FirstName.ShouldBe(dto.FirstName);
            customerUpdated.LastName.ShouldNotBe(customer.LastName);
            customerUpdated.LastName.ShouldBe(dto.LastName);
            customerUpdated.PhoneNumber.ShouldBe(dto.PhoneNumber);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            Authenticate(_userId, client);
            var customer = _customers[1];

            var response = await client.Request($"{Path}/{customer.Id}").DeleteAsync();
            var customerFromDb = await dbContext.Customers.Where(c => c.Id == customer.Id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            customerFromDb.ShouldNotBeNull();
            customerFromDb.Active.ShouldBeFalse();
        }

        public async Task InitializeAsync()
        {
            _customers = await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private const string Path = "contacts-module/customers";
        private readonly Guid _userId;
        private List<Customer> _customers = [];

        public CustomerControllerTests(TestApplicationFactory<Program> factory, TestContactsDbContext dbContext)
            : base(factory, dbContext)
        {
            _userId = dbContext.UserId;
        }

        private async Task<List<Customer>> AddSampleData()
        {
            var customers = GetSampleData();
            var customer1 = customers[0];
            var customer2 = customers[1];
            await dbContext.Customers.AddAsync(customer1);
            await dbContext.Customers.AddAsync(customer2);
            await dbContext.SaveChangesAsync();
            return customers;
        }

        private List<Customer> GetSampleData()
        {
            var customer1 = new Customer { Id = Guid.NewGuid(), FirstName = "Johny", LastName = "Bravo", PhoneNumber = "123456789", UserId = _userId, Company = false, Active = true };
            var customer2 = new Customer { Id = Guid.NewGuid(), FirstName = "George", LastName = "Tanker", PhoneNumber = "123456789", UserId = _userId, Company = false, Active = true };
            return new List<Customer> { customer1, customer2 };
        }
    }
}
