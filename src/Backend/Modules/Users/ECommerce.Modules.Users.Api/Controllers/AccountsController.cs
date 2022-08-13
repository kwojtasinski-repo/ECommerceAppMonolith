using ECommerce.Modules.Users.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Shared.Infrastructure.Filters.ActionFilters;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Abstractions.Auth;

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

        [HttpPatch("{id:guid}/active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<JsonWebToken>> ChangeUserActiveAsync(Guid id, ChangeUserActive changeUserActive)
        {
            changeUserActive.UserId = id;
            return Ok(await _identityService.ChangeUserActiveAsync(changeUserActive));
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


        [HttpPut("{id:guid}/policies")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<JsonWebToken>> UpdatePoliciesAsync(Guid id, UpdatePolicies updatePolicies)
        {
            updatePolicies.UserId = id;
            return Ok(await _identityService.UpdatePoliciesAsync(updatePolicies));
        }
    }
}
