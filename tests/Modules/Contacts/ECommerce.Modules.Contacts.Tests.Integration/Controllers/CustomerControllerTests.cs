using ECommerce.Modules.Contacts.Core.DAL;
using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
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
