using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Items;
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
    internal class ItemsController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync()
        {
            var itemDtos = await _queryDispatcher.QueryAsync(new GetItems());
            return Ok(itemDtos);
        }


        [HttpGet("{itemId:guid}")]
        public async Task<ActionResult<ItemDetailsDto>> GetAsync(Guid itemId)
        {
            var itemDto = await _queryDispatcher.QueryAsync(new GetItem(itemId));
            return OkOrNotFound(itemDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateItem command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(UpdateItem command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(DeleteItem command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}
