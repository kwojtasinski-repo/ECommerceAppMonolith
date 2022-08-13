using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Application.Orders.Queries;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    [Authorize]
    internal class CartController : BaseController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IContext _context;

        public CartController(IQueryDispatcher queryDispatcher, IContext context)
        {
            _queryDispatcher = queryDispatcher;
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAllMyItemsInCartAsync()
        {
            var orderItems = await _queryDispatcher.QueryAsync(new GetAllMyItemsInCart(_context.Identity.Id));
            return Ok(orderItems);
        }
    }
}
