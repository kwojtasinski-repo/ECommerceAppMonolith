using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Events;
using ECommerce.Modules.Users.Core.Exceptions;
using ECommerce.Modules.Users.Core.Mappings;
using ECommerce.Modules.Users.Core.Repositories;
using ECommerce.Shared.Abstractions.Auth;
using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace ECommerce.Modules.Users.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;

        public IdentityService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
            IAuthManager authManager, IClock clock, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authManager = authManager;
            _clock = clock;
            _messageBroker = messageBroker;
        }

        public async Task<JsonWebToken> ChangeCredentialsAsync(ChangeCredentialsDto dto)
        {
            var action = "";
            var changeEmail = !string.IsNullOrWhiteSpace(dto.NewEmail);
            var changePassword = !string.IsNullOrWhiteSpace(dto.NewPassword);
            JsonWebToken token = null;
            var email = dto.OldEmail.ToLowerInvariant();
            var user = await _userRepository.GetAsync(email);

            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            if (changeEmail)
            {
                action = "changeEmail";
            }

            if (changePassword)
            {
                action = "changePassword";
            }

            if (changeEmail && changePassword)
            {
                action = "changeEmailAndPassword";
            }

            switch (action)
            {
                case "changeEmail":
                    await ChangeEmail(user, dto.NewEmail);
                    break;
                case "changePassword" :
                    await ChangePassword(user, dto);
                    break;
                case "changeEmailAndPassword":
                    await ChangeEmailAndPassword(user, dto);
                    break;
                default :
                    break;
            }

            if (!string.IsNullOrEmpty(action))
            {
                token = GenerateToken(user);
            }

            return token;
        }

        public async Task<AccountDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user is null ? null : user.AsAccountDto();
        }

        public async Task<JsonWebToken> SignInAsync(SignInDto dto)
        {
            var user = await _userRepository.GetAsync(dto.Email.ToLowerInvariant());
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            VerifyPassword(user.Password, dto.Password);

            if (!user.IsActive)
            {
                throw new UserNotActiveException(user.Id);
            }

            var jwt = GenerateToken(user);

            return jwt;
        }

        public async Task SignUpAsync(SignUpDto dto)
        {
            dto.Id = Guid.NewGuid();
            var email = dto.Email.ToLowerInvariant();
            CheckPassword(dto.Password);
            var user = await _userRepository.GetAsync(email);

            if (user is not null)
            {
                throw new EmailInUseException();
            }

            var password = _passwordHasher.HashPassword(default, dto.Password);
            user = new User
            {
                Id = dto.Id,
                Email = email,
                Password = password,
                Role = dto.Role?.ToLowerInvariant() ?? "user",
                CreatedAt = _clock.CurrentDate(),
                IsActive = true,
                Claims = dto.Claims ?? new Dictionary<string, IEnumerable<string>>()
            };
            await _userRepository.AddAsync(user);
            await _messageBroker.PublishAsync(new SignedUp(user.Id, email));
        }

        private static void CheckPassword(string password)
        {
            Regex pattern = new ("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\w\\W]{8,}$");

            if (!pattern.IsMatch(password))
            {
                throw new InvalidPasswordException();
            }
        }

        private JsonWebToken GenerateToken(User user)
        {
            var jwt = _authManager.CreateToken(user.Id.ToString(), user.Role, claims: user.Claims);
            jwt.Email = user.Email;
            return jwt;
        }

        private async Task ChangeEmail(User user, string newEmail)
        {
            var email = newEmail.ToLowerInvariant();
            var userExists = await _userRepository.GetAsync(email);

            if (userExists is not null)
            {
                throw new EmailInUseException();
            }

            user.Email = email;
            await _userRepository.UpdateAsync(user);
        }

        private async Task ChangePassword(User user, ChangeCredentialsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewPasswordConfirm))
            {
                throw new PasswordsAreNotSameException();
            }

            if (dto.NewPassword != dto.NewPasswordConfirm)
            {
                throw new PasswordsAreNotSameException();
            }

            VerifyPassword(user.Password, dto.OldPassword);
            CheckPassword(dto.NewPassword);

            var password = _passwordHasher.HashPassword(default, dto.NewPassword);
            user.Password = password;
            await _userRepository.UpdateAsync(user);
        }

        private async Task ChangeEmailAndPassword(User user, ChangeCredentialsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewPasswordConfirm))
            {
                throw new PasswordsAreNotSameException();
            }

            if (dto.NewPassword != dto.NewPasswordConfirm)
            {
                throw new PasswordsAreNotSameException();
            }

            var email = dto.NewEmail.ToLowerInvariant();
            var userExists = await _userRepository.GetAsync(email);

            if (userExists is not null)
            {
                throw new EmailInUseException();
            }

            VerifyPassword(user.Password, dto.OldPassword);
            CheckPassword(dto.NewPassword);

            user.Email = email;
            var password = _passwordHasher.HashPassword(default, dto.NewPassword);
            user.Password = password;
            await _userRepository.UpdateAsync(user);
        }

        private void VerifyPassword(string correctPassword, string password)
        {
            if (_passwordHasher.VerifyHashedPassword(default, correctPassword, password) ==
                PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var accounts = new List<AccountDto>();
            foreach (var user in users)
            {
                accounts.Add(user.AsAccountDto());
            }

            return accounts;
        }

        public async Task<IEnumerable<AccountDto>> GetAllByEmailAsync(string email)
        {
            var users = await _userRepository.GetAllByEmailAsync(email);

            var accounts = new List<AccountDto>();
            foreach (var user in users)
            {
                accounts.Add(user.AsAccountDto());
            }

            return accounts;
        }

        public async Task<JsonWebToken> ChangeUserActiveAsync(ChangeUserActive changeUserActive)
        {
            var user = await _userRepository.GetAsync(changeUserActive.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(changeUserActive.UserId);
            }

            user.IsActive = changeUserActive.Active;
            await _userRepository.UpdateAsync(user);
            return GenerateToken(user);
        }

        public async Task<JsonWebToken> UpdatePoliciesAsync(UpdatePolicies updatePolicies)
        {
            var user = await _userRepository.GetAsync(updatePolicies.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(updatePolicies.UserId);
            }

            user.Role = updatePolicies.Role;

            if (updatePolicies.Claims is not null && user.Claims is not null)
            {
                if (user.Claims.TryGetValue("permissions", out var claims))
                {
                    var permissions = new List<string>(claims);

                    foreach (var claim in updatePolicies.Claims)
                    {
                        if (!permissions.Any(c => c == claim))
                        {
                            permissions.Add(claim);
                        }
                    }

                    foreach (var claim in claims)
                    {
                        if (!updatePolicies.Claims.Any(c => c == claim))
                        {
                            permissions.Remove(claim);
                        }
                    }

                    user.Claims["permissions"] = permissions;
                }
            }

            await _userRepository.UpdateAsync(user);
            return GenerateToken(user);
        }
    }
}
