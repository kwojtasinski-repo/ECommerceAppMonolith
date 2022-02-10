using ECommerce.Modules.Items.Application.Commands.ItemsSale;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.ItemsSale;
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
    internal class ItemsSaleController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemsSaleController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemSaleDto>>> GetAllAsync()
        {
            var itemsSale = await _queryDispatcher.QueryAsync(new GetItemsSale());
            return Ok(itemsSale);
        }

        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [HttpGet("{itemSaleId:guid}")]
        public async Task<ActionResult<ItemSaleDetailsDto>> GetAsync(Guid itemSaleId)
        {
            var itemSale = await _queryDispatcher.QueryAsync(new GetItemSale(itemSaleId));
            return Ok(itemSale);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateItemSale command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { itemSaleId = command.ItemSaleId }, null);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(UpdateItemSale command) 
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{itemSaleId:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid itemSaleId)
        {
            await _commandDispatcher.SendAsync(new DeleteItemSale(itemSaleId));
            return Ok();
        }
    }
}
