using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Abstractions.Auth;
using ECommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Api.Controllers
{
    internal class AccountController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly IContext _context;

        public AccountController(IIdentityService identityService, IContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AccountDto>> GetAsync()
            => OkOrNotFound(await _identityService.GetAsync(_context.Identity.Id));

        [HttpPost("sign-up")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> SignUpAsync(SignUpDto dto)
        {
            await _identityService.SignUpAsync(dto);
            return Ok();
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<JsonWebToken>> SignInAsync(SignInDto dto)
            => Ok(await _identityService.SignInAsync(dto));

        [HttpPost("change-credentials")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<JsonWebToken>> ChangeCredentialsAsync(ChangeCredentialsDto dto)
            => Ok(await _identityService.ChangeCredentialsAsync(dto));
    }
}
