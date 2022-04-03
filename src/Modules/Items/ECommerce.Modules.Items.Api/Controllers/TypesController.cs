using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Types;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Api.Controllers
{
    [Authorize(Policy)]
    internal class TypesController : BaseController
    {
        private const string Policy = "items";
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TypesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllAsync()
        {
            var typeDtos = await _queryDispatcher.QueryAsync(new GetTypes());
            return Ok(typeDtos);
        }

        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [HttpGet("{typeId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TypeDto>> GetAsync(Guid typeId)
        {
            var typeDto = await _queryDispatcher.QueryAsync(new GetType(typeId));
            return OkOrNotFound(typeDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> AddAsync(CreateType command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { typeId = command.TypeId }, null);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> UpdateAsync(UpdateType command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{typeId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> DeleteAsync(Guid typeId)
        {
            await _commandDispatcher.SendAsync(new DeleteType(typeId));
            return Ok();
        }
    }
}
