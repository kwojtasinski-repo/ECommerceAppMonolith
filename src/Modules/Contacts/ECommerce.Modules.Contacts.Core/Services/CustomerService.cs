using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Exceptions.Customers;
using ECommerce.Modules.Contacts.Core.Mappings;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Shared.Abstractions.Validators;

namespace ECommerce.Modules.Contacts.Core.Services
{
    internal class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CustomerDto> _validator;

        public CustomerService(ICustomerRepository customerRepository, IValidator<CustomerDto> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task AddAsync(CustomerDto dto)
        {
            _validator.Validate(dto);
            dto.Id = Guid.NewGuid();
            await _customerRepository.AddAsync(dto.AsEntity());
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);

            if (customer is null)
            {
                throw new CustomerNotFoundException(id);
            }

            await _customerRepository.DeleteAsync(customer);
        }

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => c.AsDto()).ToList();
        }

        public async Task<CustomerDetailsDto> GetAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return customer.AsDetailsDto();
        }

        public async Task UpdateAsync(CustomerDto dto)
        {
            _validator.Validate(dto);
            var customer = await _customerRepository.GetAsync(dto.Id);

            if (customer is null)
            {
                throw new CustomerNotFoundException(dto.Id);
            }

            await _customerRepository.UpdateAsync(dto.AsEntity());
        }
    }
}
