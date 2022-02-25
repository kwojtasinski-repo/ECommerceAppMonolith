using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    [Authorize]
    internal class OrdersController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IContext _context;

        public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IContext context)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var orders = await _queryDispatcher.QueryAsync(new GetOrdersByUserId { UserId = _context.Identity.Id });
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OrderDetailsDto>> GetAsync(Guid orderId)
        {
            var order = await _queryDispatcher.QueryAsync(new GetOrder { OrderId = orderId });
            return OkOrNotFound(order);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> PostAsync(CreateOrder command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { orderId = command.Id }, null);
        }

        [HttpPatch("positions/add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> AddPositionToOrderAsync(AddOrderItemToOrder command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpPatch("positions/delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeletePositionToOrderAsync(DeleteOrderItemFromOrder command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{orderId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteAsync(Guid orderId)
        {
            await _commandDispatcher.SendAsync(new DeleteOrder(orderId));
            return Ok();
        }
    }
}
