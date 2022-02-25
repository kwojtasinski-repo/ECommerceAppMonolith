using ECommerce.Modules.Sales.Application.Payments.Commands;
using ECommerce.Modules.Sales.Application.Payments.DTO;
using ECommerce.Modules.Sales.Application.Payments.Queries;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    [Authorize]
    internal class PaymentsController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IContext _context;

        public PaymentsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IContext context)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllAsync()
        {
            var payments = await _queryDispatcher.QueryAsync(new GetPayments(_context.Identity.Id));
            return Ok(payments);
        }

        [HttpGet("{paymentId:guid}")]
        [ActionName("GetAsync")] // blad z metoda GetAsync (nie moze jej znalezc podczas CrateAtAction())
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<PaymentDto>> GetAsync(Guid paymentId)
        {
            var payment = await _queryDispatcher.QueryAsync(new GetPayment(paymentId));
            return OkOrNotFound(payment);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> PostAsync(CreatePayment command)
        {
            await _commandDispatcher.SendAsync(command);
            return CreatedAtAction(nameof(GetAsync), new { paymentId = command.Id }, null);
        }

        [HttpDelete("{paymentId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteAsync(Guid paymentId)
        {
            await _commandDispatcher.SendAsync(new DeletePayment(paymentId));
            return Ok();
        }
    }
}
