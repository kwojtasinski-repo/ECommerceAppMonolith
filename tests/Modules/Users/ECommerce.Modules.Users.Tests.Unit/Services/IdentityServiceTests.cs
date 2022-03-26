using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Exceptions;
using ECommerce.Modules.Users.Core.Repositories;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Abstractions.Auth;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Users.Tests.Unit.Services
{
    public class IdentityServiceTests
    {
        [Fact]
        public async Task given_valid_user_should_sign_up()
        {
            var signUp = new SignUpDto { Id = Guid.NewGuid(), Email = "test@testowy.pl", Password = "Password123", Role = "user" };

            await _service.SignUpAsync(signUp);

            await _userRepository.Received(1).GetAsync(Arg.Any<string>());
            _passwordHasher.Received(1).HashPassword(Arg.Any<User>(), Arg.Any<string>());
        }

        [Fact]
        public async Task given_email_already_in_use_should_throw_an_exception()
        {
            var signUp = new SignUpDto { Id = Guid.NewGuid(), Email = "test@testowy.pl", Password = "Password1234", Role = "user" };
            var expectedException = new EmailInUseException();
            _userRepository.GetAsync(signUp.Email).Returns(new User());

            var exception = await Record.ExceptionAsync(() => _service.SignUpAsync(signUp));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<EmailInUseException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_password_should_throw_an_exception_when_sign_up()
        {
            var signUp = new SignUpDto { Id = Guid.NewGuid(), Email = "test@testowy.pl", Password = "password", Role = "user" };
            var expectedException = new InvalidPasswordException();
            _userRepository.GetAsync(signUp.Email).Returns(new User());

            var exception = await Record.ExceptionAsync(() => _service.SignUpAsync(signUp));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPasswordException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_valid_sign_in_dto_should_return_json_token()
        {
            var dto = new SignInDto { Email = "test@teser.pl", Password = "Password123!" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.Email, dto.Password, role, claims);
            _userRepository.GetAsync(dto.Email.ToLowerInvariant()).Returns(user);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var token = CreateToken(role);
            _authManager.CreateToken(Arg.Any<string>(), role, claims: claims).Returns(token);

            var jsonToken = await _service.SignInAsync(dto);

            jsonToken.ShouldNotBeNull();
            jsonToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
            jsonToken.Email.ShouldNotBeNullOrWhiteSpace();
            jsonToken.Email.ShouldBe(dto.Email);
        }

        [Fact]
        public async Task given_invalid_email_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "test@teser.pl", Password = "password" };
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _service.SignInAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCredentialsException>();
            exception.Message.ShouldBe(exception.Message);
        }

        [Fact]
        public async Task given_invalid_password_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "test@teser.pl", Password = "password" };
            var expectedException = new InvalidCredentialsException();
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.Email, dto.Password, role, claims);
            _userRepository.GetAsync(dto.Email.ToLowerInvariant()).Returns(user);

            var exception = await Record.ExceptionAsync(() => _service.SignInAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCredentialsException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_inactived_user_should_throw_an_exception()
        {
            var dto = new SignInDto { Email = "test@teser.pl", Password = "password" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.Email, dto.Password, role, claims);
            user.IsActive = false;
            _userRepository.GetAsync(dto.Email.ToLowerInvariant()).Returns(user);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var expectedException = new UserNotActiveException(user.Id);

            var exception = await Record.ExceptionAsync(() => _service.SignInAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserNotActiveException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((UserNotActiveException) exception).UserId.ShouldBe(expectedException.UserId);
        }

        [Fact]
        public async Task given_valid_dto_should_change_email()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", NewEmail = "email@email.com" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, "password", role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user);
            var token = CreateToken(role);
            _authManager.CreateToken(Arg.Any<string>(), role, claims: claims).Returns(token);

            var jwt = await _service.ChangeCredentialsAsync(dto);

            jwt.ShouldNotBeNull();
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task given_valid_dto_should_change_password()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", OldPassword="password", NewPassword="Abc1234!sadgs", NewPasswordConfirm = "Abc1234!sadgs" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var token = CreateToken(role);
            _authManager.CreateToken(Arg.Any<string>(), role, claims: claims).Returns(token);

            var jwt = await _service.ChangeCredentialsAsync(dto);

            jwt.ShouldNotBeNull();
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task given_valid_dto_should_change_email_and_password()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", NewEmail="em12@gmail.com", OldPassword = "password", NewPassword = "Abc1234!sadgs", NewPasswordConfirm = "Abc1234!sadgs" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var token = CreateToken(role);
            _authManager.CreateToken(Arg.Any<string>(), role, claims: claims).Returns(token);

            var jwt = await _service.ChangeCredentialsAsync(dto);

            jwt.ShouldNotBeNull();
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task given_valid_dto_without_new_values_should_return_null_jwt()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", NewEmail = "", NewPasswordConfirm = "", NewPassword = "", OldPassword = "" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user);

            var jwt = await _service.ChangeCredentialsAsync(dto);

            jwt.ShouldBeNull();
            await _userRepository.Received(0).UpdateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task given_invalid_old_email_should_throw_an_exception()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "" };
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _service.ChangeCredentialsAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_password_when_change_credentials_should_throw_an_exception()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", OldPassword = "password", NewPassword = "new", NewPasswordConfirm = "new" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user); _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Failed);
            var expectedException = new InvalidCredentialsException();

            var exception = await Record.ExceptionAsync(() => _service.ChangeCredentialsAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_new_password_when_change_credentials_should_throw_an_exception()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", OldPassword = "password", NewPassword = "new", NewPasswordConfirm = "new" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user); _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var expectedException = new InvalidPasswordException();

            var exception = await Record.ExceptionAsync(() => _service.ChangeCredentialsAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_null_new_password_confirm_when_change_credentials_should_throw_an_exception()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", OldPassword = "password", NewPassword = "Newa1323Abcsa" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user); _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var expectedException = new PasswordsAreNotSameException();

            var exception = await Record.ExceptionAsync(() => _service.ChangeCredentialsAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_different_new_passwords_when_change_credentials_should_throw_an_exception()
        {
            var dto = new ChangeCredentialsDto { OldEmail = "tester@gmail.com", OldPassword = "password", NewPassword = "Newa1323Abcsa", NewPasswordConfirm = "Newa1323AbcsaA!" };
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>();
            var user = GetSampleUser(dto.OldEmail, dto.OldPassword, role, claims);
            _userRepository.GetAsync(dto.OldEmail.ToLowerInvariant()).Returns(user); _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            _passwordHasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>()).Returns(PasswordVerificationResult.Success);
            var expectedException = new PasswordsAreNotSameException();

            var exception = await Record.ExceptionAsync(() => _service.ChangeCredentialsAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
        }

        private static User GetSampleUser(string email, string password, string role, Dictionary<string, IEnumerable<string>> claims)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Password = password,
                Role = role,
                Claims = claims
            };
        }

        private static JsonWebToken CreateToken(string role = null)
        {
            var token = new JsonWebToken
            {
                Id = Guid.NewGuid().ToString(),
                AccessToken = Guid.NewGuid().ToString("N"),
                Claims = new Dictionary<string, IEnumerable<string>>(),
                Expires = 100000000,
                Role = role != null ? role : "user"
            };
            return token;
        }

        private readonly IIdentityService _service;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;

        public IdentityServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _passwordHasher = Substitute.For<IPasswordHasher<User>>();
            _authManager = Substitute.For<IAuthManager>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _service = new IdentityService(_userRepository, _passwordHasher, _authManager, _clock);
        }
    }
}