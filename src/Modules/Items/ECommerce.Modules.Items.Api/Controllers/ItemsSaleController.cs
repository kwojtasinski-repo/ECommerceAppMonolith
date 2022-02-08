using ECommerce.Modules.Items.Application.Commands.ItemsSale;
using ECommerce.Shared.Abstractions.Commands;
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

        public ItemsSaleController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [HttpGet("{itemSaleId:guid}")]
        public async Task<ActionResult> GetAsync(Guid itemSaleId)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateItemSale command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { itemSaleId = command.ItemSaleId }, null);
        }
    }
}
