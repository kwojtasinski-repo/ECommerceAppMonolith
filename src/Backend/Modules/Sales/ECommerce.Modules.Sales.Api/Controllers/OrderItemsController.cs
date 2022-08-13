using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    [Authorize]
    internal class OrderItemsController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public OrderItemsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{orderItemId:guid}")]
        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderItemDto>> GetAsync(Guid orderItemId)
        {
            var orderItem = await _queryDispatcher.QueryAsync(new GetOrderItem { OrderItemId = orderItemId });
            return OkOrNotFound(orderItem);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> PostAsync(CreateOrderItem command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { orderItemId = command.OrderItemId }, null);
        }

        [HttpPost("multi")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> AddAllAsync(CreateOrderItems command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{orderItemId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteAsync(Guid orderItemId)
        {
            await _commandDispatcher.SendAsync(new DeleteOrderItem(orderItemId));
            return Ok();
        }
    }
}
