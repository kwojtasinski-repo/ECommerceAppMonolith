using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    internal class OrdersController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderDetailsDto>> GetAsync(Guid orderId)
        {
            var order = await _queryDispatcher.QueryAsync(new GetOrder { OrderId = orderId });
            return OkOrNotFound(order);
        }
    }
}
