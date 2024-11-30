using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Modules.Users.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Users.Tests.Integration.Controllers
{
    public class AccountsControllerTests : BaseTest
    {
        [Fact]
        public async Task given_valid_user_should_change_role()
        {
            var signUpDto = new SignUpDto { Email = "test-test@testowy.pl", Password = "Password123", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };
            await _identityService.SignUpAsync(signUpDto);
            var userAdded = await _identityService.SignInAsync(new SignInDto { Email = signUpDto.Email, Password = signUpDto.Password });
            Authenticate(Guid.NewGuid(), client);
            var policiesToUpdate = new UpdatePolicies { Role = "admin" };

            await client.Request($"{Path}/{userAdded.Id}/policies").PutJsonAsync(policiesToUpdate);

            var userUpdated = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == Guid.Parse(userAdded.Id));
            userUpdated.Role.ShouldBe(policiesToUpdate.Role);
        }

        [Fact]
        public async Task given_valid_user_should_change_activity()
        {
            var signUpDto = new SignUpDto { Email = "test@testowyAbc123.pl", Password = "Password123", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };
            await _identityService.SignUpAsync(signUpDto);
            var userAdded = await _identityService.SignInAsync(new SignInDto { Email = signUpDto.Email, Password = signUpDto.Password });
            Authenticate(Guid.NewGuid(), client);
            var policiesToUpdate = new ChangeUserActive { Active = false };

            await client.Request($"{Path}/{userAdded.Id}/active").PatchJsonAsync(policiesToUpdate);

            var userUpdated = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == Guid.Parse(userAdded.Id));
            userUpdated.IsActive.ShouldBeFalse();
        }

        private const string Path = "users-module/accounts";
        private readonly IIdentityService _identityService;

        public AccountsControllerTests(TestApplicationFactory<Program> factory, TestUsersDbContext dbContext)
            : base(factory, dbContext)
        {
            _identityService = factory.Services.GetService<IIdentityService>();
        }
    }
}
