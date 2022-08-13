using ECommerce.Modules.Items.Application.Commands.ItemSales;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.ItemSales;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Items.Api.Controllers
{
    [Authorize(Policy)]
    internal class ItemSalesController : BaseController
    {
        private const string Policy = "item-sale";
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemSalesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ItemSaleDto>>> GetAllAsync()
        {
            var itemsSale = await _queryDispatcher.QueryAsync(new GetItemsSale());
            return Ok(itemsSale);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ItemSaleDto>>> GetAllFilteredByNameAsync([FromQuery] string name)
        {
            var itemsSale = await _queryDispatcher.QueryAsync(new GetAllFilteredByName(name));
            return Ok(itemsSale);
        }

        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [HttpGet("{itemSaleId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ItemSaleDetailsDto>> GetAsync(Guid itemSaleId)
        {
            var itemSale = await _queryDispatcher.QueryAsync(new GetItemSale(itemSaleId));
            return OkOrNotFound(itemSale);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> AddAsync(CreateItemSale command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { itemSaleId = command.ItemSaleId }, null);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateItemSale command) 
        {
            await _commandDispatcher.SendAsync(new UpdateItemSale(id, command.ItemCost, command.CurrencyCode));
            return Ok();
        }

        [HttpDelete("{itemSaleId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> DeleteAsync(Guid itemSaleId)
        {
            await _commandDispatcher.SendAsync(new DeleteItemSale(itemSaleId));
            return Ok();
        }
    }
}
