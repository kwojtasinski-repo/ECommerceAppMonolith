using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Exceptions;
using ECommerce.Modules.Users.Tests.Integration.Common;
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
    public class AccountControllerTests : BaseTest
    {
        [Fact]
        public async Task given_valid_user_should_sign_up()
        {
            var signUpDto = new SignUpDto { Email = "test@testowy.pl", Password = "Password123", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };

            await client.Request($"{Path}/sign-up").PostJsonAsync(signUpDto);

            dbContext.Users.Count().ShouldBeGreaterThan(0);
            var user = await dbContext.Users.Where(u => u.Email == signUpDto.Email).SingleOrDefaultAsync();
            user.ShouldNotBeNull();
            user.Email.ShouldBe(signUpDto.Email);
        }

        [Fact]
        public async Task given_existing_email_when_sign_up_should_return_bad_request()
        {
            var signUpDto = new SignUpDto { Email = "test2@testowy.pl", Password = "Password123!", Role = "user", Claims = new Dictionary<string, IEnumerable<string>>() };
            await dbContext.Users.AddAsync(CreateSampleUser(signUpDto));
            await dbContext.SaveChangesAsync();
            var expectedException = new EmailInUseException();

            var response = await client.Request($"{Path}/sign-up").AllowHttpStatus("400").PostJsonAsync(signUpDto);
            
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
            var user = CreateSampleUser(signInDto);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var response = await client.Request($"{Path}/sign-in").PostJsonAsync(signInDto);
            var jsonToken = await response.GetJsonAsync<JsonWebToken>();

            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task given_not_existing_user_when_sign_in_should_return_bad_request()
        {
            var signInDto = new SignInDto { Email = "test4@testowy.pl", Password = "Password123a@" };
            var expectedException = new InvalidCredentialsException();

            var response = await client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

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
            var user = CreateSampleUserWithHashedPassword(signInDto.Email, "PassW0RD!@12", "user", []);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync(); 
            var expectedException = new InvalidCredentialsException();

            var response = await client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

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
            var user = CreateSampleUser(signInDto);
            user.IsActive = false;
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            var expectedException = new UserNotActiveException(user.Id);

            var response = await client.Request($"{Path}/sign-in").AllowHttpStatus("400").PostJsonAsync(signInDto);

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
            var user = CreateSampleUserWithHashedPassword(dto.OldEmail, dto.OldPassword, "user", []);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            Authenticate(Guid.NewGuid(), client);

            var response = await client.Request($"{Path}/change-credentials").PostJsonAsync(dto);

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
            var user = CreateSampleUserWithHashedPassword(dto.OldEmail, dto.OldPassword, "user", []);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            Authenticate(Guid.NewGuid(), client);

            var response = await client.Request($"{Path}/change-credentials").PostJsonAsync(dto);
            var responseSignIn = await client.Request($"{Path}/sign-in").PostJsonAsync(new SignInDto { Email = dto.NewEmail, Password = dto.NewPassword });
            var jsonToken = await response.GetJsonAsync<JsonWebToken>();
            var jsonTokenSigIn = await responseSignIn.GetJsonAsync<JsonWebToken>();

            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonToken.Email.ShouldBe(dto.NewEmail.ToLowerInvariant());
            jsonTokenSigIn.ShouldNotBeNull();
            jsonTokenSigIn.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonTokenSigIn.Email.ShouldBe(dto.NewEmail.ToLowerInvariant());
        }

        private static User CreateSampleUser(SignUpDto dto, string role = "user")
        {
            return CreateSampleUser(dto.Email, dto.Email, role, []);
        }

        private User CreateSampleUser(SignInDto dto, string role = "user")
        {
            return CreateSampleUserWithHashedPassword(dto.Email, dto.Password, role, []);
        }

        private User CreateSampleUserWithHashedPassword(string email, string password, string role, Dictionary<string, IEnumerable<string>> claims)
        {
            return CreateSampleUser(email, _passwordHasher.HashPassword(default, password), role, claims);
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
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountControllerTests(TestApplicationFactory<Program> factory, TestUsersDbContext dbContext)
            : base(factory, dbContext)
        {
            _passwordHasher = factory.Services.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
        }
    }
}
