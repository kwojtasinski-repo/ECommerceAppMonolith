using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Types;
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
    internal class TypesController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TypesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllAsync()
        {
            var typeDtos = await _queryDispatcher.QueryAsync(new GetTypes());
            return Ok(typeDtos);
        }

        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [HttpGet("{itemId:guid}")]
        public async Task<ActionResult<TypeDto>> GetAsync(Guid typeId)
        {
            var typeDto = await _queryDispatcher.QueryAsync(new GetType(typeId));
            return OkOrNotFound(typeDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateType command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { typeId = command.TypeId }, null);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(UpdateType command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(DeleteType command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}
