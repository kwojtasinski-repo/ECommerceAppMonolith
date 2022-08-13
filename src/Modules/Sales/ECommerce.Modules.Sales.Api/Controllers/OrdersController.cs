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

        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var orders = await _queryDispatcher.QueryAsync(new GetOrdersByUserId { UserId = _context.Identity.Id });
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        [ActionName("GetAsync")] // error at CreateAction cannot find method
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderDetailsDto>> GetAsync(Guid orderId)
        {
            var order = await _queryDispatcher.QueryAsync(new GetOrder { OrderId = orderId });
            return OkOrNotFound(order);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> PostAsync(CreateOrder command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { orderId = command.Id }, null);
        }

        [HttpPatch("{id:guid}/positions/add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> AddPositionToOrderAsync(Guid id, AddOrderItemToOrder command)
        {
            await _commandDispatcher.SendAsync(new AddOrderItemToOrder(id, command.ItemSaleId));
            return Ok();
        }

        [HttpPatch("{id:guid}/positions/delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeletePositionFromOrderAsync(Guid id, DeleteOrderItemFromOrder command)
        {
            await _commandDispatcher.SendAsync(new DeleteOrderItemFromOrder(id, command.OrderItemId));
            return Ok();
        }

        [HttpPatch("{id:guid}/customer/change")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> ChangeCustomerAsync(Guid id, ChangeCustomerInOrder command)
        {
            await _commandDispatcher.SendAsync(new ChangeCustomerInOrder(id, command.CustomerId));
            return Ok();
        }

        [HttpPatch("{id:guid}/currency/change")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> ChangeCurrencyAsync(Guid id, ChangeCurrencyInOrder command)
        {
            await _commandDispatcher.SendAsync(new ChangeCurrencyInOrder(id, command.CurrencyCode));
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
