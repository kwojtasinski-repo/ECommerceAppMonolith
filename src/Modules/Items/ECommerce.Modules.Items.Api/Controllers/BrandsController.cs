using ECommerce.Modules.Items.Application.Commands.Brands;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Brands;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Api.Controllers
{
    internal class BrandsController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public BrandsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllAsync()
        {
            var brandDtos = await _queryDispatcher.QueryAsync(new GetBrands());
            return Ok(brandDtos);
        }

        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [HttpGet("{brandId:guid}")]
        public async Task<ActionResult<BrandDto>> GetAsync(Guid brandId)
        {
            var brandDto = await _queryDispatcher.QueryAsync(new GetBrand(brandId));
            return OkOrNotFound(brandDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateBrand command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { brandId = command.BrandId }, null);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(UpdateBrand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(DeleteBrand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}
