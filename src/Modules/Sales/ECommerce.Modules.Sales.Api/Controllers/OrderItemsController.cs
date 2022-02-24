using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Sales.Api.Controllers
{
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
        public async Task<ActionResult<OrderItemDto>> GetAsync(Guid orderItemId)
        {
            var orderItem = await _queryDispatcher.QueryAsync(new GetOrderItem { OrderItemId = orderItemId });
            return OkOrNotFound(orderItem);
        }
    }
}
