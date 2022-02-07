using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Shared.Abstractions.Commands;
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

        public TypesController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateType command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}
