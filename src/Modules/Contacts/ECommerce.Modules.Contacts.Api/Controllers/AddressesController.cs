using ECommerce.Modules.Contacts.Core.DTO;
using ECommerce.Modules.Contacts.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Api.Controllers
{
    internal class AddressesController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id:guid}")]
        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        public async Task<ActionResult<AddressDto>> GetAsync(Guid id)
        {
            var address = await _addressService.GetAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(AddressDto address)
        {
            await _addressService.AddAsync(address);
            return CreatedAtAction(nameof(GetAsync), new { id = address.Id }, null);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(AddressDto address)
        {
            await _addressService.UpdateAsync(address);
            return Ok();
        }
    }
}
