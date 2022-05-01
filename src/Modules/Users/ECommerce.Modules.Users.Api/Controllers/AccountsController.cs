using ECommerce.Modules.Users.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Shared.Abstractions.Filters.ActionFilters;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Abstractions.Contexts;

namespace ECommerce.Modules.Users.Api.Controllers
{
    internal class AccountsController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IContext _context;

        public AccountsController(IIdentityService identityService, IContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [CheckPermissions("users")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAsync()
        {
            return Ok(await _identityService.GetAllAsync());
        }

        [HttpGet("search")]
        [Authorize(Roles = "admin")]
        [CheckPermissions("users")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllByEmailAsync([FromQuery] string email)
        {
            return Ok(await _identityService.GetAllByEmailAsync(email));
        }
    }
}
