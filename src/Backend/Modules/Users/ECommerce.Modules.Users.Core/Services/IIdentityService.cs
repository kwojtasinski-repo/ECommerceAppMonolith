using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Shared.Abstractions.Auth;

namespace ECommerce.Modules.Users.Core.Services
{
    internal interface IIdentityService
    {
        Task<AccountDto> GetAsync(Guid id);
        Task<JsonWebToken> SignInAsync(SignInDto dto);
        Task SignUpAsync(SignUpDto dto);
        Task<JsonWebToken> ChangeCredentialsAsync(ChangeCredentialsDto dto);
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task<IEnumerable<AccountDto>> GetAllByEmailAsync(string email);
        Task<JsonWebToken> ChangeUserActiveAsync(ChangeUserActive changeUserActive);
        Task<JsonWebToken> UpdatePoliciesAsync(UpdatePolicies updatePolicies);
    }
}
