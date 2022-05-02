using ECommerce.Modules.Users.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Shared.Infrastructure.Filters.ActionFilters;
using ECommerce.Modules.Users.Core.Services;

namespace ECommerce.Modules.Users.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [CheckPermissions("users")]
    internal class AccountsController : BaseController
    {
        private readonly IIdentityService _identityService;

        public AccountsController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAsync()
        {
            return Ok(await _identityService.GetAllAsync());
        }

        [HttpGet("search")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllByEmailAsync([FromQuery] string email)
        {
            return Ok(await _identityService.GetAllByEmailAsync(email));
        }

        [HttpPatch("active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> ChangeUserActiveAsync(ChangeUserActive changeUserActive)
        {
            await _identityService.ChangeUserActiveAsync(changeUserActive);
            return Ok();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<AccountDto>> GetUserAsync(Guid id)
        {
            return OkOrNotFound(await _identityService.GetAsync(id));
        }
    }
}
