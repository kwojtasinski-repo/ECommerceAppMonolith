using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Modules.Contacts.Core.Services;
using ECommerce.Modules.Contacts.Core.Validators;
using ECommerce.Shared.Abstractions.Validators;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Contacts.Tests.Unit.Services
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task given_valid_customer_should_add()
        {
            var customer = new CustomerDto
            {
                FirstName = "John",
                LastName = "Tester",
                Company = false,
                PhoneNumber = "123456789"
            };

            await _service.AddAsync(customer);

            await _customerRepository.Received(1).AddAsync(Arg.Any<Customer>());
        }

        [Fact]
        public async Task given_valid_customer_should_update()
        {
            var customer = CreateCustomer();
            var dto = new CustomerDto
            {
                Id = customer.Id,
                FirstName = "John",
                LastName = "Tester",
                Company = false,
                PhoneNumber = "525123789"
            };
            _customerRepository.GetAsync(dto.Id).Returns(customer);

            await _service.UpdateAsync(dto);

            await _customerRepository.Received(1).UpdateAsync(Arg.Any<Customer>());
        }

        private readonly CustomerService _service;
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CustomerDto> _validator; 

        public CustomerServiceTests()
        {
            _validator = new CustomerValidator();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _service = new CustomerService(_customerRepository, _validator);
        }

        private Customer CreateCustomer()
        {
            return new Customer
            {
                Id = Guid.NewGuid(),
                Company = false,
                Active = true,
                UserId = Guid.NewGuid(),
                FirstName = "George",
                LastName = "Post",
                PhoneNumber = "123456789"
            };
        }
    }
}
