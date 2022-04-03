using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Api.Controllers
{
    [Authorize]
    internal class AddressesController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id:guid}")]
        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AddressDto>> GetAsync(Guid id)
        {
            var address = await _addressService.GetAsync(id);
            return OkOrNotFound(address);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PostAsync(AddressDto address)
        {
            await _addressService.AddAsync(address);
            return CreatedAtAction(nameof(GetAsync), new { id = address.Id }, null);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PutAsync(AddressDto address)
        {
            await _addressService.UpdateAsync(address);
            return Ok();
        }
    }
}
