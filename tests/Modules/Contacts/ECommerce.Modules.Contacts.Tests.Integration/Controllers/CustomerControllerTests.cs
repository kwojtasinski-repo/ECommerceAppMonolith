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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Contacts.Tests.Integration.Controllers
{
    public class CustomerControllerTests : IClassFixture<TestApplicationFactory<Program>>,
           IClassFixture<TestContactsDbContext>
    {
        [Fact]
        public async Task given_valid_user_id_should_return_address()
        {
            var customers = await AddSampleData();
            Authenticate(_userId);

            var response = (await _client.Request($"{Path}/me").GetAsync());
            var addressesFromDb = await response.GetJsonAsync<IEnumerable<CustomerDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            addressesFromDb.ShouldNotBeNull();
            addressesFromDb.Count().ShouldBeGreaterThan(0);
            addressesFromDb.Count().ShouldBe(customers.Count);
        }

        [Fact]
        public async Task given_valid_id_should_return_address()
        {
            var customers = await AddSampleData();
            var customer = customers.FirstOrDefault();
            Authenticate(_userId);

            var response = (await _client.Request($"{Path}/{customer.Id}").GetAsync());
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
            Authenticate(_userId);

            var response = await _client.Request($"{Path}").PostJsonAsync(dto);
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var customerFromDb = _dbContext.Customers.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();

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
            var customers = await AddSampleData();
            var customer = customers[1];
            Authenticate(_userId);
            var dto = new CustomerDto { Id = customer.Id, FirstName = "Michael", LastName = "Employer", PhoneNumber = "123456789", Company = false };

            var response = await _client.Request($"{Path}").PutJsonAsync(dto);
            var customerUpdated = await (await _client.Request($"{Path}/{customer.Id}").GetAsync()).GetJsonAsync<CustomerDto>();

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
            var customers = await AddSampleData();
            Authenticate(_userId);
            var customer = customers[1];

            var response = await _client.Request($"{Path}/{customer.Id}").DeleteAsync();
            var customerFromDb = await _dbContext.Customers.Where(c => c.Id == customer.Id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            customerFromDb.ShouldNotBeNull();
            customerFromDb.Active.ShouldBeFalse();
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "currencies" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private const string Path = "contacts-module/customers";
        private readonly IFlurlClient _client;
        private readonly ContactsDbContext _dbContext;
        private readonly Guid _userId;

        public CustomerControllerTests(TestApplicationFactory<Program> factory, TestContactsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
            _userId = dbContext.UserId;
        }

        private async Task<List<Customer>> AddSampleData()
        {
            var customers = GetSampleData();
            var customer1 = customers[0];
            var customer2 = customers[1];
            await _dbContext.Customers.AddAsync(customer1);
            await _dbContext.Customers.AddAsync(customer2);
            await _dbContext.SaveChangesAsync();
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
