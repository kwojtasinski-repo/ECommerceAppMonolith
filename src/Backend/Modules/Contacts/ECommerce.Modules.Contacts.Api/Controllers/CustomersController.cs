using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Services;
using ECommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Contacts.Api.Controllers
{
    [Authorize]
    internal class CustomersController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IContext _context;

        public CustomersController(ICustomerService customerService, IContext context)
        {
            _customerService = customerService;
            _context = context;
        }

        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllByUserIdAsync()
        {
            var customers = await _customerService.GetAllByUserAsync(_context.Identity.Id);
            return Ok(customers);
        }

        [HttpGet("{id:guid}")]
        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerDetailsDto>> GetAsync(Guid id)
        {
            var customer = await _customerService.GetAsync(id);
            return OkOrNotFound(customer);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostAsync(CustomerDto customerDto)
        {
            customerDto.UserId = _context.Identity.Id;
            await _customerService.AddAsync(customerDto);
            return CreatedAtAction(nameof(GetAsync), new { id = customerDto.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PutAsync(Guid id, CustomerDto customerDto)
        {
            customerDto.Id = id;
            await _customerService.UpdateAsync(customerDto);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return Ok();
        }
    }
}
