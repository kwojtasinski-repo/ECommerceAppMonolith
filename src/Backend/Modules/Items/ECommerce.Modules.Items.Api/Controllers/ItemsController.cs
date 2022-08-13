using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Items;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Items.Api.Controllers
{
    [Authorize(Policy)]
    internal class ItemsController : BaseController
    {
        private const string Policy = "items";
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync()
        {
            var itemDtos = await _queryDispatcher.QueryAsync(new GetItems());
            return Ok(itemDtos);
        }

        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [HttpGet("{itemId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ItemDetailsDto>> GetAsync(Guid itemId)
        {
            var itemDto = await _queryDispatcher.QueryAsync(new GetItem(itemId));
            return OkOrNotFound(itemDto);
        }

        [HttpGet("not-put-up-for-sale")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllNotPutUpAsync()
        {
            var itemDtos = await _queryDispatcher.QueryAsync(new GetItemsNotPutUpForSale());
            return Ok(itemDtos);
        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> AddAsync(CreateItem command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { itemId = command.ItemId }, null);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> UpdateAsync(Guid id, UpdateItem command)
        {
            await _commandDispatcher.SendAsync(new UpdateItem(id, command.ItemName, command.Description, command.BrandId, command.TypeId,
                command.Tags, command.ImagesUrl));
            return Ok();
        }

        [HttpDelete("{itemId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> DeleteAsync(Guid itemId)
        {
            await _commandDispatcher.SendAsync(new DeleteItem(itemId));
            return Ok();
        }
    }
}
