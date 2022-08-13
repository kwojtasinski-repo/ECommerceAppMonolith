using ECommerce.Modules.Users.Core.DAL;
using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Exceptions;
using ECommerce.Shared.Abstractions.Auth;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Users.Tests.Integration.Controllers
{
    public class AccountControllerTests : BaseIntegrationTest, IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestUsersDbContext>
    {
        [Fact]
        public async Task given_valid_user_should_sign_up()
        {
            var signUpDto = new SignUpDto { Email = "test@testowy.pl", Password = "Password123", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };

            await _client.Request($"{Path}/sign-up").PostJsonAsync(signUpDto);

            _dbContext.Users.Count().ShouldBeGreaterThan(0);
            var user = await _dbContext.Users.Where(u => u.Email == signUpDto.Email).SingleOrDefaultAsync();
            user.ShouldNotBeNull();
            user.Email.ShouldBe(signUpDto.Email);
        }

        [Fact]
        public async Task given_existing_email_when_sign_up_should_return_bad_request()
        {
            var signUpDto = new SignUpDto { Email = "test2@testowy.pl", Password = "Password123!", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };
            await _dbContext.Users.AddAsync(CreateSampleUser(signUpDto.Email, signUpDto.Email, "user", new Dictionary<string, IEnumerable<string>>()));
            await _dbContext.SaveChangesAsync();
            var expectedException = new EmailInUseException();

            var response = await _client.Request($"{Path}/sign-up").AllowHttpStatus("400").PostJsonAsync(signUpDto);
            
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int) HttpStatusCode.BadRequest);
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_user_should_sign_in_and_return_token()
        {
            var signInDto = new SignInDto { Email = "test3@testowy.pl", Password = "Password412" };
            var user = CreateSampleUser(signInDto.Email,
                _passwordHasher.HashPassword(default, signInDto.Password), "user", new Dictionary<string, IEnumerable<string>>());
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var response = await _client.Request($"{Path}/sign-in").PostJsonAsync(signInDto);
            var jsonToken = await response.GetJsonAsync<JsonWebToken>();

            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task given_not_existing_user_when_sign_in_should_return_bad_request()
        {
            var signInDto = new SignInDto { Email = "test4@testowy.pl", Password = "Password123a@" };
            var expectedException = new InvalidCredentialsException();

            var response = await _client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int) HttpStatusCode.BadRequest);
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_password_when_sign_in_should_return_bad_request()
        {
            var signInDto = new SignInDto { Email = "test5@testowy.pl", Password = "Password21" };
            var user = CreateSampleUser(signInDto.Email,
                _passwordHasher.HashPassword(default, "PassW0RD!@12"), "user", new Dictionary<string, IEnumerable<string>>());
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync(); 
            var expectedException = new InvalidCredentialsException();

            var response = await _client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int) HttpStatusCode.BadRequest);
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_not_active_user_when_sign_in_should_return_bad_request()
        {
            var signInDto = new SignInDto { Email = "test6@testowy.pl", Password = "password" };
            var user = CreateSampleUser(signInDto.Email,
                _passwordHasher.HashPassword(default, signInDto.Password), "user", new Dictionary<string, IEnumerable<string>>());
            user.IsActive = false;
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var expectedException = new UserNotActiveException(user.Id);

            var response = await _client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int) HttpStatusCode.BadRequest);
            var errors = await response.GetJsonAsync<Dictionary<string, IEnumerable<ErrorMessage>>>();
            var exception = errors["errors"].FirstOrDefault();
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_new_email_and_password_should_change_credentials()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "test123@testowy.pl", OldPassword = "password", NewEmail = "email@gmail.com", NewPassword = "NewP@As1W2RD", NewPasswordConfirm = "NewP@As1W2RD" };
            var user = CreateSampleUser(dto.OldEmail,
                _passwordHasher.HashPassword(default, dto.OldPassword), "user", new Dictionary<string, IEnumerable<string>>());
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            Authenticate(Guid.NewGuid(), _client);

            var response = await _client.Request($"{Path}/change-credentials").PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var jsonToken = await response.GetJsonAsync<JsonWebToken>();
            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonToken.Email.ShouldBe(dto.NewEmail);
        }

        [Fact]
        public async Task given_valid_new_email_and_password_should_change_credentials_and_sign_in_with_new_password_and_email()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "test123@testowy.pl", OldPassword = "password", NewEmail = "emailPasw90675@gmail.com", NewPassword = "NewP@As1W2RD", NewPasswordConfirm = "NewP@As1W2RD" };
            var user = CreateSampleUser(dto.OldEmail,
                _passwordHasher.HashPassword(default, dto.OldPassword), "user", new Dictionary<string, IEnumerable<string>>());
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            Authenticate(Guid.NewGuid(), _client);

            var response = await _client.Request($"{Path}/change-credentials").PostJsonAsync(dto);
            var responseSignIn = await _client.Request($"{Path}/sign-in").PostJsonAsync(new SignInDto { Email = dto.NewEmail, Password = dto.NewPassword });
            var jsonToken = await response.GetJsonAsync<JsonWebToken>();
            var jsonTokenSigIn = await responseSignIn.GetJsonAsync<JsonWebToken>();

            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonToken.Email.ShouldBe(dto.NewEmail.ToLowerInvariant());
            jsonTokenSigIn.ShouldNotBeNull();
            jsonTokenSigIn.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonTokenSigIn.Email.ShouldBe(dto.NewEmail.ToLowerInvariant());
        }

        private static User CreateSampleUser(string email, string password, string role, Dictionary<string, IEnumerable<string>> claims)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Password = password,
                Role = role,
                Claims = claims,
            };
        }

        private const string Path = "users-module/account";
        private readonly UsersDbContext _dbContext;
        private readonly IFlurlClient _client;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountControllerTests(TestApplicationFactory<Program> factory, TestUsersDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
            _passwordHasher = factory.Services.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
        }
    }
}
